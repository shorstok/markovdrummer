using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using markov_drummer.Properties;

namespace markov_drummer.Config
{
    [DataContract]
    public abstract class GlobalSettings : INotifyPropertyChanged
    {
        public static string Appendix = "markov-drummer";

        public static string GetOrCreateSettingsPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Appendix);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            try
            {
                // This will raise an exception if the path is read only or do not have access to view the permissions.                
                Directory.GetAccessControl(path);
            }
            catch (UnauthorizedAccessException)
            {
                throw new InvalidDataException($"GetOrCreateSettingsPath: Cannot write to folder `{path}`");
            }

            return path;

        }

        /*
         * NB about INotifyPropertyChanged: 
         * it's ok to bind view to singleton-held properties, since
         * PropertyChangedEventManager is WeakEventListener and so GlobalSettings singleton
         * instance would not hold a reference to bound view.
         */

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    [DataContract]
    public abstract class GlobalSettings<T> : GlobalSettings where T : GlobalSettings<T>, new()
    {
        private static T _current = null;

        // Silencing warning <StaticMemberInGenericType> because it's perfectly OK that Lock would be separate in different GlobalSettings<T>
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        public static T Current
        {
            get
            {
                return _current = _current ?? Load();
            }
        }

        protected static string SettingsFilename
        {
            get
            {
                var appendix = typeof(T).Name;

                return Path.Combine(GetOrCreateSettingsPath(), $"settings_{appendix}.xml");
            }
        }

        protected GlobalSettings()
        {
            RestoreDefaults(new StreamingContext());
        }
        
        [OnDeserializing]
        private void RestoreDefaults(StreamingContext context)
        {
            List<object> fieldsOrProperties =
                typeof(T).GetFields()
                    .Where(f => f.GetCustomAttributes(true).OfType<DefaultGlobalSettingValue>().Any())
                    .Cast<object>()
                    .ToList();

            fieldsOrProperties.AddRange(typeof(T).GetProperties()
                    .Where(f => f.GetCustomAttributes(true).OfType<DefaultGlobalSettingValue>().Any()));

            foreach (var info in fieldsOrProperties)
            {
                DefaultGlobalSettingValue def = null;

                var pi = info as PropertyInfo;
                var fi = info as FieldInfo;

                if (pi != null)
                    def = pi.GetCustomAttributes(true).OfType<DefaultGlobalSettingValue>().FirstOrDefault();
                else if (fi != null)
                    def = fi.GetCustomAttributes(true).OfType<DefaultGlobalSettingValue>().FirstOrDefault();

                if (def == null)
                    continue;

                var ptype = pi?.PropertyType ?? fi.FieldType;

                var value = def.CreateNew ? Activator.CreateInstance(ptype) : def.Value;

                pi?.SetValue(this, value, null);
                fi?.SetValue(this, value);
            }
        }

        protected static T Load()
        {
            lock (Lock)
            {
                if (!File.Exists(SettingsFilename))
                    return new T();

                using (var fs = new FileStream(SettingsFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var serializer = new DataContractSerializer(typeof(T));

                    try
                    {
                        return (T)serializer.ReadObject(fs);
                    }
                    catch (XmlException)
                    {
                        return new T();
                    }
                    catch (SerializationException)
                    {
                        return new T();
                    }
                }
            }
        }

        public void Save()
        {
            lock (Lock)
            {
                var serializer = new DataContractSerializer(typeof(T));

                try
                {
                    var settings = new XmlWriterSettings { Indent = true };
                    using (var w = XmlWriter.Create(SettingsFilename, settings))
                    {
                        serializer.WriteObject(w, this);
                    }
                }
                catch (SerializationException)
                {
                }
            }

        }
    }

}
