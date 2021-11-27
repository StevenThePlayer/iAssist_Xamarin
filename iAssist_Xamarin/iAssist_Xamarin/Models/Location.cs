using System;
using System.Collections.Generic;
using System.Text;

namespace iAssist_Xamarin.Models
{
    public class MapAutocomplete
    {
        public string description { get; set; }
        public string place_id { get; set; }
    }
    public class MapDetails
    {
        public string place_id { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class MatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class MainTextMatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class StructuredFormatting
    {
        public string main_text { get; set; }
        public List<MainTextMatchedSubstring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }
    }

    public class Term
    {
        public int offset { get; set; }
        public string value { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public StructuredFormatting structured_formatting { get; set; }
        public List<Term> terms { get; set; }
        public List<string> types { get; set; }
    }

    public class RootAutocomplete
    {
        public List<Prediction> predictions { get; set; }
        public string status { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Geometry
    {
        public Location location { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string name { get; set; }
    }

    public class RootDetails
    {
        public List<object> html_attributions { get; set; }
        public Result result { get; set; }
        public string status { get; set; }
    }
}
