using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ChaiCooking.Models.Custom.AlbumAPI
{
    public class ColourTileModel
    {
        [JsonProperty("hex_colour_codes")]
        public List<TileColour> colourList { get; set; }
    }

    public partial class TileColour
    {
        public string colour { get; set; }

        public bool isHighlighted { get; set; }
    }
}
