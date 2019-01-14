using System.Collections.Generic;

namespace ELTB.Models {

        public class LanguageMetadata {

                public string Name { get; set; }

                public string Summary { get; set; }

                public string DetailsUrl { get; set; }

                public static LanguageMetadata Create(string name, string summary, string url = null) {
                        return new LanguageMetadata {
                                Name = name,
                                Summary = summary,
                                DetailsUrl = url
                        };
                }

                public static IEnumerable<LanguageMetadata> All { get; set; } = new List<LanguageMetadata>();

        }
}
