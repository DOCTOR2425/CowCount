namespace CowCount.ViewModels
{
    class CowViewModel
    {
        public int Number { get; set; }
        public int BarnNumber { get; set; }
        public int SectionNumber { get; set; }
        public string? Group { get; set; }
        public bool Gender { get; set; }
        public string? Breed { get; set; }
        public bool Imported { get; set; }
        public string? Note { get; set; }
        public List<string>? ActionLog { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime DeathDate { get; set; }
        /// <summary>
        /// Дата последнего осеменения
        /// </summary>
        public DateTime LastInseminationDate { get; set; }
        /// <summary>
        /// Дата последнего отёла
        /// </summary>
        public DateTime LastCalvingDate { get; set; }
        public string? SpermDonorNickname { get; set; }
    }
}
