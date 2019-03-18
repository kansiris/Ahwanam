using MaaAahwanam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaAahwanam.Repository.db
{
   public class NotesRepository
    {
        readonly ApiContext _dbContext = new ApiContext();

        public Note AddNotes(Note note)
        {
            _dbContext.Notes.Add(note);
            _dbContext.SaveChanges();
            return note;
        }

        public Note UpdateNotes(Note note,long notesId)
        {
            var Notedata = _dbContext.Notes.SingleOrDefault(n =>n.NotesId == notesId);
            note.NotesId = Notedata.NotesId;
            note.VendorId = Notedata.VendorId;
            note.wishlistId = Notedata.wishlistId;
            note.VendorSubId = Notedata.VendorSubId;
            _dbContext.Entry(Notedata).CurrentValues.SetValues(note);
            _dbContext.SaveChanges();
            return Notedata;
        }

        public int RemoveNotes(long notesId)
        {
            var data = _dbContext.Notes.Where(n => n.NotesId == notesId).FirstOrDefault();
            _dbContext.Notes.Remove(data);
            return _dbContext.SaveChanges();
        }

        public Collabrator AddCollabrator(Collabrator collabrator)
        {
            _dbContext.Collabrator.Add(collabrator);
            _dbContext.SaveChanges();
            return collabrator;
        }

        public int RemoveCollabrator(long collabratorId)
        {
            var getdata = _dbContext.Collabrator.Where(m => m.Id == collabratorId).FirstOrDefault();
            _dbContext.Collabrator.Remove(getdata);
            return _dbContext.SaveChanges();
        }
    }
}
