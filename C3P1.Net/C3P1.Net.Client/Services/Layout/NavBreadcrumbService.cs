namespace C3P1.Net.Client.Services.Layout
{
    public class NavBreadcrumbService
    {
        public List<Node> Breadcrumbs { get; set; } = [];
        public bool Visible { get; set; } = false;

        public event Action? OnChange;

        public void SetVisible(bool isVisible)
        {
            Visible = isVisible;
            NotifyStateChanged();
        }

        public void Reset()
        {
            Breadcrumbs = [];
            Visible = false;

            NotifyStateChanged();
        }

        public void SetBreadcrumbs(List<Node> breadcrumbs)
        {
            Breadcrumbs = breadcrumbs;
            Visible = true;

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
    public class Node
    {
        public string? Text { get; set; }
        public string? Link { get; set; }
        public bool Active { get; set; }
    }
}
