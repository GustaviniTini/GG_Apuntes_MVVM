namespace GG_Apuntes.Views;

public partial class GG_AllApuntesPage : ContentPage
{
    public GG_AllApuntesPage()
    {
        InitializeComponent();
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        notesCollection.SelectedItem = null;
    }
}