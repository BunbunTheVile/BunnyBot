namespace BunnyBot
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = "";

        // TODO: implement handling of faulty lines
        public static List<Note> GetAllNotes()
        {
            var notes = new List<Note>();

            var noteLines = File.ReadAllLines(Resources.NotesPath).ToList();
            foreach (var noteLine in noteLines)
            {
                var note = new Note();
                if (!int.TryParse(noteLine.Split("#")[1], out var id)) continue;
                note.Id = id;
                note.Content = noteLine.Split("#")[2].Trim();
                notes.Add(note);
            }

            return notes;
        }

        /// <summary>
        /// Generates a new Note object with an unused ID
        /// </summary>
        public static Note GetNewNote()
        {
            var existingNotes = GetAllNotes();
            var newId = existingNotes.Count > 0
                ? existingNotes.OrderByDescending(x => x.Id).FirstOrDefault()!.Id + 1
                : 1;
            return new Note { Id = newId };
        }

        public override string ToString()
        {
            return $"#{this.Id}\t{this.Content}";
        }
    }
}
