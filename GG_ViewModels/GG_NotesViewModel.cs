using CommunityToolkit.Mvvm.Input;
using GG_Apuntes.GG_ViewModels;
using GG_Apuntes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GG_Apuntes.GG_ViewModels;

internal class GG_NotesViewModel : IQueryAttributable
{
    public ObservableCollection<GG_ViewModels.GG_NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public GG_NotesViewModel()
    {
        AllNotes = new ObservableCollection<GG_ViewModels.GG_NoteViewModel>(Models.GG_Notes.LoadAll().Select(n => new GG_NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<GG_ViewModels.GG_NoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.GG_NotePage));
    }

    private async Task SelectNoteAsync(GG_ViewModels.GG_NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.GG_NotePage)}?load={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            GG_NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            GG_NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new GG_NoteViewModel(Models.GG_Notes.Load(noteId)));
        }
    }
}