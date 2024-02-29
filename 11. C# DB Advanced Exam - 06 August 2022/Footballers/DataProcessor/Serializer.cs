namespace Footballers.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .Select(c => new
                {
                    CoachName = c.Name,
                    Footballers = c.Footballers.Select(f => new
                    {
                        f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(f => f.Name)
                    .ToList()
                })
                .ToList()
                .OrderByDescending(c => c.Footballers.Count)
                .ThenBy(c => c.CoachName)
                .ToList();

            var doc = new XDocument();
            var root = new XElement("Coaches");
            doc.Add(root);

            foreach (var coach in coaches)
            {
                var coachElement = new XElement("Coach");
                coachElement.SetAttributeValue("FootballersCount", coach.Footballers.Count);
                root.Add(coachElement);

                var coachName = new XElement("CoachName", coach.CoachName);
                coachElement.Add(coachName);

                var footballers = new XElement("Footballers");
                coachElement.Add(footballers);

                foreach (var footballer in coach.Footballers)
                {
                    var footballerElement = new XElement("Footballer");
                    footballers.Add(footballerElement);

                    var name = new XElement("Name", footballer.Name);
                    footballerElement.Add(name);

                    var position = new XElement("Position", footballer.Position);
                    footballerElement.Add(position);
                }
            }

            return doc.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var footballers = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .Select(t => new
                {
                    t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                    .ThenBy(tf => tf.Footballer.Name)
                    .Select(tf => new
                    {
                        FootballerName = tf.Footballer.Name,
                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("MM/dd/yyyy"),
                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("MM/dd/yyyy"),
                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                        PositionType = tf.Footballer.PositionType.ToString()
                    })
                    .ToList()
                })
                .ToList()
                .OrderByDescending(t => t.Footballers.Count)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToList();

            var json = JsonConvert.SerializeObject(footballers, Formatting.Indented);

            return json.ToString();
        }
    }
}
