using System;
using FundRaiser.Common.Models;

namespace FundRaiser.Common.Dto
{
    public class UpdateDto
    {
        public UpdateDto() { }

        public UpdateDto(Update update)
        {
            Id = update.Id;
            ProjectId = update.ProjectId;
            Title = update.Title;
            Descripiton = update.Description;
            PostDate = update.PostDate;
        }
        
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Descripiton { get; set; }
        public DateTime PostDate { get; set; }
    }
}