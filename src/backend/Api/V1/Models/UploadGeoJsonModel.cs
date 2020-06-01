using Humanizer;
using System;

namespace Api.V1.Models
{
    public class UploadGeoJsonModel
    {
        public UploadGeoJsonModel(Guid id,string fileName, long size, int features, string md5)
        {
            FileName = fileName;
            Size = new Size(size);
            Features = features;
            Md5 = md5;
            Id = id;
        }

        public Guid Id { get; set; }
        public string FileName { get; private set; }
        public Size Size { get; private set; }
        public int Features { get; private set; }
        public string Md5 { get; private set; }
    }

    public class Size
    {
        public Size(long byteSize)
        {
            var size = byteSize.Bytes();

            Megabytes = size.Megabytes;
            Kilobytes = size.Kilobytes;
            Bytes = size.Bytes;
            Bits = size.Bits;
        }

        public double Megabytes { get; }
        public double Kilobytes { get; }
        public double Bytes { get; }
        public long Bits { get; }
    }
}
