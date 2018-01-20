using System.Net;
using System.IO;
using System.Collections.Generic;

namespace Application
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            MapGenerator.Configration config = new MapGenerator.Configration();
            config.setImageSize(2000, 2000);
            config.setImageScale(2);
            config.setZoomLevel(10);
            config.setLocation(38.41, -110.79);
            config.setSatelliteImage(true);

            MapGenerator.Fetch(config);
        }
    }

    class MapGenerator
    {
        public class Configration
        {
            Dictionary<String, String> param = new Dictionary<String, String>();

            public void setImageSize(int width, int height)
            {
                param.Add("size", width + "x" + height);
            }

            public void setImageScale(int scale)
            {
                param.Add("scale", scale.ToString());
            }

            public void setLocation(double longitude, double latitude)
            {
                param.Add("center", longitude + "," + latitude);
            }

            public void setZoomLevel(int zoom)
            {
                param.Add("zoom", zoom.ToString());
            }

            public void setSatelliteImage(bool enable)
            {
                param.Add("maptype", enable ? "satellite" : "");
            }

            public String toString()
            {
                String result = "";
                foreach (String key in param.Keys)
                {
                    result += key + "=" + param[key] + "&";
                }
                return result;
            }
        }

        public static void Fetch(MapGenerator.Configration config)
        {
            String url = "https://maps.googleapis.com/maps/api/staticmap?" + config.toString();
            String filename = "/Users/sydney/Desktop/map_image.jpg";

            Uri uri = new Uri(url);

            HttpWebRequest request = WebRequest.CreateHttp(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream input = response.GetResponseStream();

            using (Stream file = File.Create(filename))
            {
                MapGenerator.CopyStream(input, file);
            }

        }

        static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}
