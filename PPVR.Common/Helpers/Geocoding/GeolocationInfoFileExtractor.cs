using ExifLib;
using System;
using System.IO;

namespace PPVR.Common.Helpers.Geocoding
{
    public class GeolocationInfoFileExtractor
    {
        #region Constructors

        public GeolocationInfoFileExtractor(string fileName)
        {
            _reader = new ExifReader(fileName);
            ValidateFile();

            Latitude = GetLatitude();
            Longitude = GetLongitude();
        }

        public GeolocationInfoFileExtractor(Stream file)
        {
            _reader = new ExifReader(file);
            ValidateFile();

            Latitude = GetLatitude();
            Longitude = GetLongitude();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            _reader.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Fields

        private readonly ExifReader _reader;
        private double[] _exifLatitude;
        private string _exifLatitudeRef;
        private double[] _exifLongitude;
        private string _exifLongitudeRef;

        #endregion

        #region Properties

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        #endregion

        #region Private Methods

        private static double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + minutes / 60 + seconds / 3600;
        }

        private double GetLatitude()
        {
            var result = ConvertDegreeAngleToDouble(_exifLatitude[0], _exifLatitude[1], _exifLatitude[2]);

            return _exifLatitudeRef == "S" ? result * -1 : result;
        }

        private double GetLongitude()
        {
            var result = ConvertDegreeAngleToDouble(_exifLongitude[0], _exifLongitude[1], _exifLongitude[2]);

            return _exifLongitudeRef == "W" ? result * -1 : result;
        }

        private void ValidateFile()
        {
            if (!(_reader.GetTagValue(ExifTags.GPSLatitude, out _exifLatitude) &&
                  _reader.GetTagValue(ExifTags.GPSLatitudeRef, out _exifLatitudeRef) &&
                  _reader.GetTagValue(ExifTags.GPSLongitude, out _exifLongitude) &&
                  _reader.GetTagValue(ExifTags.GPSLongitudeRef, out _exifLongitudeRef)))
            {
                throw new Exception("A foto não possui as informações de geolocalização.");
            }
        }

        #endregion
    }
}