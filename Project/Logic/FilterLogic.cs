public static class FilterLogic
{


    public static bool CheckAppliedFilters()
    {
        if (Filter.AppliedFilters == null)
        {
            MovieModel MovieModel = new(0, "", new DateTime(), "", "", 0, 0, new List<CategoryModel>());
            Filter.AppliedFilters = MovieModel;
            return false;
        }
        else if (Filter.AppliedFilters.Title == ""
            && Filter.AppliedFilters.ReleaseDate == DateTime.MinValue
            && Filter.AppliedFilters.Categories.Count == 0
            && Filter.AppliedFilters.Price == 0.0)
        {
            return false;
        }
        return true;
    }

    public static List<MovieModel> ApplyFilters()
    {
        MovieModel? filters = Filter.AppliedFilters;

        // Console.ReadKey();
        return null;
    }
}