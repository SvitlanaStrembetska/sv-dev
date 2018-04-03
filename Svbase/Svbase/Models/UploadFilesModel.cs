using System.Collections.Generic;

namespace Svbase.Models
{
    public class UploadFilesModel
    {
        public string FileName { get; set; }
        public List<Dictionary<string, object>> DataList { get; set; }
    }
}