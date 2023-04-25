public static class FilterLogic
{


    public static bool CheckAppliedFilters()
    {
        if (Filter.AppliedFilters == null)
        {
            MovieModel MovieModel = new(0, "", new DateTime(), "", "", 0, 0, new List<CategoryModel>(), new List<string>());
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
        MoviesLogic MoviesLogic = new();
        MovieModel? filters = Filter.AppliedFilters;
        List<MovieModel> filteredMovies = MoviesLogic.AllMovies();

        // Check filterable properties of filter model
        if (filters.Title != "")
        {
            filteredMovies = MoviesLogic.GetByTitle(filters.Title);
        }

        if (filters.Price != 0.0)
        {
            filteredMovies = MoviesLogic.GetByPrice(filters.Price, filteredMovies);
        }

        if (filters.ReleaseDate != DateTime.MinValue)
        {
            filteredMovies = MoviesLogic.GetByTimeSlots(filters.ReleaseDate, filteredMovies);
        }

        if (filters.Categories.Count != 0)
        {
            filteredMovies = MoviesLogic.GetByCategories(filters.Categories, filteredMovies);
        }

        foreach (var p in filteredMovies)
        {
            Console.WriteLine(p.Title);
        }

        return filteredMovies;
    }
}