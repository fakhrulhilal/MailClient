using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Mail.Library.Configuration
{
    /// <summary>
    /// INI configuration file parser
    /// </summary>
    public class IniConfiguration
    {
        /// <summary>
        /// File path to INI file
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Default section name if first found key-value pair isn't started with section.
        /// Default to 'General' if not specified.
        /// </summary>
        public string DefaultSection { get; set; }

        /// <summary>
        /// All configurations
        /// </summary>
        private List<IniBase> _configurations = new List<IniBase>();

        /// <summary>
        /// Initialize new INI configuration file
        /// </summary>
        /// <param name="path">Full path to INI file</param>
        /// <param name="defaultSection">Default section name if first found key-value pair isn't started with section</param>
        public IniConfiguration(string path, string defaultSection) : this()
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrEmpty(defaultSection)) throw new ArgumentNullException(nameof(defaultSection));
            defaultSection = defaultSection.Trim();
            if (!Regex.IsMatch(defaultSection, @"^\w+$")) throw new ArgumentException("Not valid section name", nameof(defaultSection));
            Path = path;
            DefaultSection = defaultSection;
        }

        /// <summary>
        /// Initialize new INI configuration file
        /// </summary>
        public IniConfiguration()
        {
            DefaultSection = "General";
        }

        /// <summary>
        /// Initialize new INI configuration file
        /// </summary>
        /// <param name="path">Full path to INI file</param>
        public IniConfiguration(string path) : this(path, "General")
        { }

        /// <summary>
        /// Parse INI lines
        /// </summary>
        /// <param name="lines">Split INI lines</param>
        public void Parse(string[] lines)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            if (string.IsNullOrEmpty(DefaultSection)) throw new ArgumentNullException(nameof(DefaultSection));
            _configurations.Clear();
            if (lines.Length < 1) return;
            int lineNumber = 0;
            var output = new List<IniBase>();
            IniSection lastSection = null;
            foreach (string line in lines)
            {
                lineNumber++;
                if (string.IsNullOrEmpty(line)) continue;
                IniComment comment;
                if (IniLineParser.ParseComment(line, out comment))
                {
                    comment.Position = lineNumber;
                    output.Add(comment);
                    continue;
                }

                IniSection section;
                if (IniLineParser.ParseSection(line, out section))
                {
                    var existing = output.OfType<IniSection>()
                        .FirstOrDefault(config => config.Name.Equals(section.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (existing != null) lastSection = existing;
                    else
                    {
                        lastSection = section;
                        section.Position = lineNumber;
                        output.Add(section);
                    }

                    continue;
                }

                IniPair pair;
                if (IniLineParser.ParseKeyValuePair(line, out pair))
                {
                    if (lastSection == null)
                    {
                        lastSection = new IniSection(DefaultSection) { Position = 1 };
                        output.Add(lastSection);
                    }

                    var existingPair =
                        lastSection.Pairs.LastOrDefault(
                            existing => existing.Key.Equals(pair.Key, StringComparison.CurrentCultureIgnoreCase));
                    if (existingPair != null)
                    {
                        existingPair.Position = lineNumber;
                        existingPair.Key = pair.Key;
                        existingPair.Value = pair.Value;
                    }
                    else
                    {
                        pair.Position = lineNumber;
                        lastSection.Pairs.Add(pair);
                    }
                }
            }

            _configurations = output;
        }

        /// <summary>
        /// Parse INI lines separated by EOL
        /// </summary>
        /// <param name="lines"></param>
        public void Parse(string lines)
        {
            if (string.IsNullOrEmpty(lines)) throw new ArgumentNullException(nameof(lines));
            Parse(lines.Split('\n'));
        }

        /// <summary>
        /// Parse INI file as specified in <see cref="Path"/>
        /// </summary>
        public void Parse()
        {
            if (string.IsNullOrEmpty(Path)) throw new ArgumentNullException(nameof(Path), "File path to INI file is required");
            if (!File.Exists(Path)) throw new FileNotFoundException("File not found or not readable", Path);
            Parse(File.ReadAllText(Path));
        }

        /// <summary>
        /// Get config value
        /// </summary>
        /// <typeparam name="TValue">Data type of value</typeparam>
        /// <param name="section">Section name</param>
        /// <param name="key">Key name</param>
        /// <returns>Value or default if not found</returns>
        public TValue GetConfig<TValue>(string section, string key)
        {
            if (string.IsNullOrEmpty(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (_configurations.Count < 1) return default(TValue);
            var config =
                (from cfg in _configurations.OfType<IniSection>()
                 where cfg.Name.Equals(section, StringComparison.CurrentCultureIgnoreCase)
                 from pair in cfg.Pairs
                 where pair.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)
                 select pair).FirstOrDefault();
            return config != null
                ? ConvertionHelper.To<TValue>(config.Value)
                : default(TValue);
        }

        /// <summary>
        /// Get config value from <see cref="DefaultSection"/>
        /// </summary>
        /// <typeparam name="TValue">Data type of value</typeparam>
        /// <param name="key">Key name</param>
        /// <returns>Value or default if not found</returns>
        public TValue GetConfig<TValue>(string key) => GetConfig<TValue>(DefaultSection, key);

        /// <summary>
        /// Get config and its pairs.
        /// If no section or pair found, then it will use default value as specified in <typeparamref name="TSection"/> metadata class.
        /// </summary>
        /// <typeparam name="TSection">Section metadata class</typeparam>
        /// <returns>Section with its pairs or null if not found</returns>
        public TSection GetConfig<TSection>() where TSection : class, new()
        {
            var config = new TSection();
            var sectionType = config.GetType();
            string sectionName = AttributeHelper.Class<TSection, IniSectionAttribute, string>(attr => attr.Name).FirstOrDefault() ??
                                 sectionType.Name;
            var section = _configurations.OfType<IniSection>()
                .FirstOrDefault(cfg => cfg.Name.Equals(sectionName, StringComparison.CurrentCultureIgnoreCase));
            var properties = ExtractProperties(sectionType);
            foreach (var property in properties)
            {
                if (!property.Info.CanWrite) continue;
                var value = property.DefaultValue;
                var found = section?.Pairs.LastOrDefault(cfg => cfg.Key.Equals(property.Key, StringComparison.CurrentCultureIgnoreCase));
                if (found != null) value = ConvertionHelper.To(property.Info.PropertyType, found.Value, property.DefaultValue);
                property.Info.SetValue(config, value);
            }

            return config;
        }

        /// <summary>
        /// Add or merge existing section with its pairs
        /// </summary>
        /// <param name="section">Section name</param>
        /// <param name="key">Pair name</param>
        /// <param name="value">Pair value</param>
        /// <param name="comment">Additional comment</param>
        public void SetConfig(string section, string key, object value, string comment = null)
        {
            if (string.IsNullOrEmpty(section)) throw new ArgumentNullException(nameof(section));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            var sectionConfig = MergeSection(section);
            MergePair(sectionConfig, key, value, comment);
        }

        /// <summary>
        /// Add or merge existing <see cref="DefaultSection"/> section with its pairs
        /// </summary>
        /// <param name="key">Pair name</param>
        /// <param name="value">Pair value</param>
        public void SetConfig(string key, object value) => SetConfig(DefaultSection, key, value);

        /// <summary>
        /// Add or merge existing section with its pairs
        /// </summary>
        /// <typeparam name="TSection">Section metadata class</typeparam>
        /// <param name="section"></param>
        public void SetConfig<TSection>(TSection section) where TSection : class, new()
        {
            if (section == null) throw new ArgumentNullException(nameof(section));
            var sectionType = section.GetType();
            string sectionName = sectionType.ClassAttribute<IniSectionAttribute, string>(attr => attr.Name).FirstOrDefault() ??
                                 sectionType.Name;
            string sectionComment = AttributeHelper.Class<TSection, IniSectionAttribute, string>(attr => attr.Comment).FirstOrDefault();
            var sectionConfig = MergeSection(sectionName, sectionComment);
            var properties = ExtractProperties(sectionType);
            foreach (var property in properties)
            {
                if (!property.Info.CanRead) continue;
                string value = property.Info.GetValue(section, null)?.ToString() ?? string.Empty;
                MergePair(sectionConfig, property.Key, value, property.Comment);
            }
        }

        /// <summary>
        /// Overwrite existing INI configuration file
        /// </summary>
        public void Write() => Write(Path, true);

        /// <summary>
        /// Write INI configuration to new path
        /// </summary>
        /// <param name="path">New path file</param>
        /// <param name="overwrite">Determine whether to overwrite existing file or not</param>
        public void Write(string path, bool overwrite)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (_configurations.Count < 1) return;
            using (var file = File.Open(
                path,
                overwrite ? FileMode.OpenOrCreate : FileMode.CreateNew,
                overwrite ? FileAccess.ReadWrite : FileAccess.Write))
            {
                file.Seek(0, SeekOrigin.Begin);
                var configs = _configurations.OrderBy(config => config.Position);
                foreach (var config in configs)
                {
                    // write section or comment
                    WriteString(file, config.ToString());
                    var section = config as IniSection;
                    if (section == null) continue;
                    // write all section pairs
                    section.Pairs.ForEach(pair => WriteString(file, pair.ToString()));
                    // append blank line for each section
                    WriteString(file, string.Empty);
                }
            }
        }

        /// <summary>
        /// Set configuration to its default value based on <see cref="IniKeyAttribute"/>
        /// </summary>
        /// <typeparam name="TConfig">Config class</typeparam>
        /// <param name="config">Config instance</param>
        public static void SetDefault<TConfig>(TConfig config) where TConfig : class, new()
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var properties = ExtractProperties(typeof(TConfig));
			foreach (var property in properties)
			{
				if (!property.Info.CanWrite || !property.HasDefaultValue) continue;
				property.Info.SetValue(config, property.DefaultValue);
			}
		}

		private void WriteString(FileStream file, string line)
        {
            var bytes = Encoding.Default.GetBytes(line + Environment.NewLine);
            file.Write(bytes, 0, bytes.Length);
        }

        private IniSection MergeSection(string sectionName, string sectionComment = null)
        {
            if (!IniLineParser.SectionRegex.IsMatch($"[{sectionName}]"))
                throw new ArgumentException($"'{sectionName}' is not valid section name");
            var sectionConfig = _configurations.OfType<IniSection>()
                .FirstOrDefault(cfg => cfg.Name.Equals(sectionName, StringComparison.CurrentCultureIgnoreCase));
            if (sectionConfig == null)
            {
                sectionConfig = new IniSection(sectionName)
                {
                    Position = (_configurations.Max(cfg => cfg.Position as int?) ?? 0) + 1
                };
                _configurations.Add(sectionConfig);
            }

            if (!string.IsNullOrEmpty(sectionComment)) sectionConfig.Comment = sectionComment;
            return sectionConfig;
        }

        private void MergePair(IniSection section, string key, object value, string comment = null)
        {
            if (!IniLineParser.PairRegex.IsMatch($"{key} = {value}"))
                throw new ArgumentException("Not valid key-value pair", nameof(key));
            var pairConfig =
                section.Pairs.LastOrDefault(pair => pair.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            if (pairConfig != null)
            {
                pairConfig.Value = value.ToString();
                if (!string.IsNullOrEmpty(comment)) pairConfig.Comment = comment;
            }
            else
            {
                pairConfig = new IniPair(key, value.ToString())
                {
                    Position = (section.Pairs.Max(pair => pair.Position as int?) ?? 0) + 1
                };
                if (!string.IsNullOrEmpty(comment)) pairConfig.Comment = comment;
                section.Pairs.Add(pairConfig);
            }
        }

        internal static List<PropertyMetadata> ExtractProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty);
            return 
                (from propertyInfo in properties
                let metaNames = type.MemberAttribute<IniKeyAttribute, string>(propertyInfo.Name, attr => attr.Name)
                let metaComments = type.MemberAttribute<IniKeyAttribute, string>(propertyInfo.Name, attr => attr.Comment)
                let metaDefaultValues = type.MemberAttribute<IniKeyAttribute, object>(propertyInfo.Name, attr => attr.DefaultValue)
                let metaHasDefaultValues = type.MemberAttribute<IniKeyAttribute, bool>(propertyInfo.Name, attr => attr.HasDefaultValue)
                select new PropertyMetadata
                {
                    Info = propertyInfo,
                    Key = metaNames.Length > 0 ? metaNames.First() : propertyInfo.Name,
                    Comment = metaComments.Length > 0 && !string.IsNullOrEmpty(metaComments[0]) ? metaComments[0] : null,
                    DefaultValue = metaDefaultValues.Length > 0
                        ? metaDefaultValues.First()
                        : propertyInfo.PropertyType == typeof(string)
                            ? null
                            : Activator.CreateInstance(propertyInfo.PropertyType),
                    HasDefaultValue = metaHasDefaultValues.Length > 0 && metaHasDefaultValues[0]
                }).ToList();
        }

        internal class PropertyMetadata
        {
            public PropertyInfo Info { get; set; }
            public string Key { get; set; }
            public string Comment { get; set; }
            public object DefaultValue { get; set; }
            public bool HasDefaultValue { get; set; }
        }
    }
}
