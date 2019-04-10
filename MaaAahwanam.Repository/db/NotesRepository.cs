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

        public List<Note> Getnote(long wishlist_id,long vendor_id)
        {
            return _dbContext.Notes.Where(n => n.wishlistId == wishlist_id && n.VendorId == vendor_id).ToList();
        }

        public Note AddNotes(Note note)
        {
            _dbContext.Notes.Add(note);
            _dbContext.SaveChanges();
            return note;
        }

        public Note UpdateNotes(long notesid,string notes)
        {
            Note note = new Note();
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            var Notedata = _dbContext.Notes.SingleOrDefault(n =>n.NotesId==notesid);
            note.NotesId = Notedata.NotesId;
            note.wishlistId = Notedata.wishlistId;
            note.wishlistItemId = Notedata.wishlistItemId;
            note.VendorId = Notedata.VendorId;
            note.Notes = notes;
            note.UpdatedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            _dbContext.Entry(Notedata).CurrentValues.SetValues(note);
            _dbContext.SaveChanges();
            return Notedata;
        }

        public int RemoveNotes(long notesId)
        {
            int i;
            var data = _dbContext.Notes.Where(n => n.NotesId == notesId).FirstOrDefault();
            if (data != null)
            {
                _dbContext.Notes.Remove(data);
                _dbContext.SaveChanges();
                i = 1;
            }
            else
                i = 0;
            return i;
        }

        public long GetcollabratorDetailsByEmail(string username)
        {
            var count = _dbContext.Collabrator.Where(m => m.Email == username).FirstOrDefault();
            if (count != null)
                return count.Id;
            else
                //count.UserLoginId = 0;
                return 0;
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
