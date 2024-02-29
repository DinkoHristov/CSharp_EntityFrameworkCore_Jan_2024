namespace Footballers.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(List<CoachDto>), new XmlRootAttribute("Coaches"));
            var coachesDto = (List<CoachDto>)serializer.Deserialize(new StringReader(xmlString));

            var result = new StringBuilder();
            foreach (var coachDto in coachesDto)
            {
                if (!IsValid(coachDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                    if (startDate == DateTime.Parse("01/01/0001") || endDate == DateTime.Parse("01/01/0001")) 
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (startDate > endDate)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = new Footballer
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = startDate,
                        ContractEndDate = endDate,
                        PositionType = (PositionType)footballerDto.PositionType,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType
                    };

                    coach.Footballers.Add(footballer);
                }

                context.Coaches.Add(coach);
                result.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.SaveChanges();
            return result.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamsDto = JsonConvert.DeserializeObject<List<TeamDto>>(jsonString);
            var allFootballers = context.Footballers.ToList();

            var result = new StringBuilder();
            foreach (var teamDto in teamsDto)
            {
                if (!IsValid(teamDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (teamDto.Trophies <= 0)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var team = new Team
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies,
                };

                foreach (var id in teamDto.Footballers.Distinct())
                {
                    if (!allFootballers.Any(f => f.Id == id))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = allFootballers.FirstOrDefault(f => f.Id == id);
                    var teamFootballer = new TeamFootballer
                    {
                        Footballer = footballer
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                context.Teams.Add(team);
                result.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }

            context.SaveChanges();
            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
