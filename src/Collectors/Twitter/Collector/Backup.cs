namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Timers;
    using Newtonsoft.Json;
    using System.Linq;

    public static class Backup
    {
        public static event Func<Message, bool> TrySend;

        private static Timer _timer;

        private static string _folder = Configuration.Collector.BackupFolder;

        private static string _extension = "bkp";

        public static void Initialize()
        {
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            _timer = new Timer(Configuration.Collector.BackupDelay);
            _timer.AutoReset = true;

            _timer.Elapsed += SearchAndTrySend;

            Log.WriteInitialized(typeof(Backup));
        }

        public static void Finalize()
        {
            _timer.Dispose();
            _timer = null;

            Log.WriteFinalized(typeof(Backup));
        }

        public static void Write(Message message)
        {
            var guid = Guid.NewGuid();

            using (var file = File.Create($"{_folder}/{guid}.{_extension}"))
            {
                var bytes = new UTF8Encoding().GetBytes(JsonConvert.SerializeObject(message));

                file.Write(bytes, 0, bytes.Length);

                Log.Write("Backup", $"Created \"{guid}.{_extension}\" backup file.");
            }

            _timer.Start();
        }

        private static void SearchAndTrySend(object _, ElapsedEventArgs __)
        {
            foreach (var file in GetBackupFiles())
            {
                if (!TrySend(JsonConvert.DeserializeObject<Message>(File.ReadAllText(file))))
                {
                    _timer.Stop();
                    _timer.Start();

                    Log.Write("Backup", $"Failed to send \"{file}\" message to message queue.");

                    break;
                }

                File.Delete(file);

                Log.Write("Backup", $"Message \"{file}\" successfully sent to message queue.");
            }

            if (GetBackupFiles().Any())
                _timer.Stop();
        }

        private static IEnumerable<string> GetBackupFiles() => Directory.GetFiles(_folder, $"*.{_extension}");
    }
}