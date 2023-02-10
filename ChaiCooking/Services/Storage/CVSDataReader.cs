using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ChaiCooking.Tools;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace ChaiCooking.Services.Storage
{
    public class CVSDataReader
    {
        System.Reflection.Assembly Assembly;

        public CVSDataReader()
        {
            Assembly = IntrospectionExtensions.GetTypeInfo(typeof(CVSDataReader)).Assembly;
        }



        /*
        List<Presentation> Presentations;
        List<Partner> Partners;
        List<Speaker> Speakers;

        public class Presentation
        {
            [Name("id")]
            public int Id { get; set; }

            [Name("name")]
            public string Name { get; set; }

            [Name("info")]
            public string Info { get; set; }

            [Name("start_time")]
            public DateTime Start { get; set; }

            [Name("end_time")]
            public DateTime End { get; set; }

			[Name("image_path")]
			public string ImagePath { get; set; }

			[Name("speaker_id")]
            public int SpeakerId { get; set; }
        }

        public class Partner
        {
            [Name("id")]
            public int Id { get; set; }

            [Name("type")]
            public int Type { get; set; }

            [Name("name")]
            public string Name { get; set; }

            [Name("info")]
            public string Info { get; set; }

            [Name("url")]
            public string Url { get; set; }

            [Name("image_path")]
            public string ImagePath { get; set; }
        }

        public class Speaker
        {
            [Name("id")]
            public int Id { get; set; }

            [Name("name")]
            public string Name { get; set; }

            [Name("company")]
            public string Company { get; set; }

            [Name("tag_line")]
            public string TagLine { get; set; }

            [Name("biog")]
            public string Biog { get; set; }

            [Name("type")]
            public string Type { get; set; }

            [Name("url")]
            public string Url { get; set; }

            [Name("image_path")]
            public string ImagePath { get; set; }

        }

        public List<TEPresentation> GetPresentations()
        {
            List<TEPresentation> presentations = new List<TEPresentation>();
            using (var csv = new CsvReader(new StreamReader(Assembly.GetManifestResourceStream("ChaiCooking.presentations.csv"))))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                Presentations = csv.GetRecords<Presentation>().ToList();
            }

			foreach (Presentation p in Presentations)
			{
				TEPresentation tEPresentation = new TEPresentation(p.Id, p.Name, p.Info, false, p.Start, p.End, -1);

				if (p.Info != "null")
				{
					tEPresentation.DetailViewAvailable = true;
				}

				if (p.SpeakerId > -1)
				{
					tEPresentation.SpeakerId = p.SpeakerId;
					tEPresentation.DetailViewAvailable = true;
				}

				if (p.ImagePath != "null")
				{
					tEPresentation.ImagePath = p.ImagePath;
				}
                else
				{
					tEPresentation.ImagePath = null;
				}

                presentations.Add(tEPresentation);

                
            }
            return presentations;
        }

        public List<TEPartner> GetPartners()
        {
            List<TEPartner> partners = new List<TEPartner>();

            using (var csv = new CsvReader(new StreamReader(Assembly.GetManifestResourceStream("ChaiCooking.partners.csv"))))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                Partners = csv.GetRecords<Partner>().ToList();
            }
  
            foreach (Partner p in Partners)
            {
                partners.Add(new TEPartner(p.Id, p.Type, p.Name, p.Info, p.Url, p.ImagePath));
            }

            return partners;
        }

        public List<TESpeaker> GetSpeakers()
        {
            List<TESpeaker> speakers = new List<TESpeaker>();

            using (var csv = new CsvReader(new StreamReader(Assembly.GetManifestResourceStream("ChaiCooking.speakers.csv"))))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                Speakers = csv.GetRecords<Speaker>().ToList();
            }

            foreach (Speaker s in Speakers)
            {
                TESpeaker spkr = new TESpeaker(s.Id, s.Name, s.Company, "null", s.TagLine, s.Biog, null, null, s.ImagePath);

                spkr.InfoSections = TextTools.TextToArray(spkr.FullText, '|');



                speakers.Add(spkr);
            }

            return speakers;
        }*/
    }


    
}
