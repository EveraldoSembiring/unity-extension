using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace UnityExtension
{
    public class DefaultFileSystem
    {
        public ActionResult WriteFile<T>(T model, string filePath, IEncryption encryption = null)
        {
            var result = new ActionResult();
            AutoCreateDirectory(filePath);

            try
            {
                string contentJson = model.ToJsonString();
                result = WriteFile(contentJson, filePath, encryption);
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }

            return result;
        }

        public ActionResult WriteFile(string content, string filePath, IEncryption encryption = null)
        {
            var result = new ActionResult();
            AutoCreateDirectory(filePath);

            try
            {
                string saveText = content;
                if(encryption != null)
                {
                    saveText = encryption.Encrypt(saveText);
                }

                using (var outputFile = new System.IO.StreamWriter(filePath))
                {
                    outputFile.Write(saveText);
                }
                result.Success();
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            
            return result;
        }

        public async Task<ActionResult> WriteAsync<T>(T model, string filePath, IEncryption encryption = null)
        {
            var result = new ActionResult();
            AutoCreateDirectory(filePath);

            try
            {
                string contentJson = model.ToJsonString();
                await WriteFileAsync(contentJson, filePath, encryption);
                result.Success();
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        public async Task<ActionResult<bool>> WriteFileAsync(string content, string filePath, IEncryption encryption = null)
        {
            var result = new ActionResult();
            AutoCreateDirectory(filePath);

            try
            {
                string saveText = content;
                if (encryption != null)
                {
                    saveText = encryption.Encrypt(saveText);
                }

                using (var outputFile = new System.IO.StreamWriter(filePath))
                {
                    await outputFile.WriteAsync(saveText);
                    result.Success();
                }
            }
            catch (Exception ex)
            {
                result.Failed(ex.Message);
            }
            return result;
        }

        public ActionResult<T> LoadFile<T>(string filePath, IEncryption encryption = null)
        {
            var loadResult = new ActionResult<T>();
            if (!File.Exists(filePath))
            {
                loadResult.Failed(message:$"File is not exist at {filePath}");
                return loadResult;
            }

            try
            {
                ActionResult<string> loadTextFileResult = LoadFile(filePath, encryption);
                loadTextFileResult.OnSuccess(modelJson =>
                {
                    T result = modelJson.ToObject<T>();
                    loadResult.Success(result);
                });
                loadTextFileResult.OnFailed(errorMessage =>
                {
                    loadResult.Failed(message:errorMessage);
                });
            }
            catch (Exception ex)
            {
                loadResult.Failed(message:ex.Message);
            }

            return loadResult;
        }

        public ActionResult<string> LoadFile(string filePath, IEncryption encryption = null)
        {
            var loadResult = new ActionResult<string>();
            if (!File.Exists(filePath))
            {
                loadResult.Failed(message:$"File is not exist at {filePath}");
                return loadResult;
            }

            try
            {
                using (var reader = new System.IO.StreamReader(filePath))
                {
                    string result = reader.ReadToEnd();
                    if(encryption != null)
                    {
                        result = encryption.Decrypt(result);
                    }
                    loadResult.Success(result);
                }
            }
            catch (Exception ex)
            {
                loadResult.Failed(message:ex.Message);
            }
            
            return loadResult;
        }

        public async Task<ActionResult<T>> LoadAsync<T>(string filePath, IEncryption encryption = null)
        {
            var loadResult = new ActionResult<T>();
            if (!File.Exists(filePath))
            {
                loadResult.Failed(message:$"File is not exist at {filePath}");
                return loadResult;
            }

            try
            {
                Task<ActionResult<string>> loadTextFileResultTask = LoadFileAsync(filePath, encryption);
                await loadTextFileResultTask;
                
                loadTextFileResultTask.Result.OnSuccess(modelJson =>
                {
                    T result = modelJson.ToObject<T>();
                    loadResult.Success(result);
                });
                loadTextFileResultTask.Result.OnFailed(errorMessage =>
                {
                    loadResult.Failed(message:errorMessage);
                });
            }
            catch (Exception ex)
            {
                loadResult.Failed(message:ex.Message);
            }
            
            return loadResult;
        }

        public async Task<ActionResult<string>> LoadFileAsync(string filePath, IEncryption encryption = null)
        {
            var loadResult = new ActionResult<string>();
            if (!File.Exists(filePath))
            {
                loadResult.Failed(message:$"File is not exist at {filePath}");
                return loadResult;
            }

            try
            {
                using (var reader = new System.IO.StreamReader(filePath))
                {
                    string result = await reader.ReadToEndAsync();
                    if (encryption != null)
                    {
                        result = encryption.Decrypt(result);
                    }
                    loadResult.Success(result);
                }
            }
            catch (Exception ex)
            {
                loadResult.Failed(message:ex.Message);
            }
            
            return loadResult;
        }

        private void AutoCreateDirectory(string filePath)
        {
            string pathDirectory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(pathDirectory))
            {
                Directory.CreateDirectory(pathDirectory);
            }
        }

        public bool CheckFileExist(string filePath)
        {
            bool retval = System.IO.File.Exists(filePath);
            return retval;
        }

        public void CopyDirectory(string sourceDir, string destinationDir, bool recursive, List<string> excludeWithFileExtension = null)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            }
            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            System.IO.Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                if (excludeWithFileExtension != null)
                {
                    if (excludeWithFileExtension.Contains(file.Extension))
                    {
                        continue;
                    }
                }
                string targetFilePath = System.IO.Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = System.IO.Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}