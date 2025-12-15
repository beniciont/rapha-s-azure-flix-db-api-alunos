import { useQuery } from '@tanstack/react-query';
import { movieService } from '@/services/movieService';
import { SearchMoviesParams } from '@/types/api';

export function useMovies(params?: SearchMoviesParams) {
  return useQuery({
    queryKey: ['movies', params],
    queryFn: () => movieService.getAll(params),
  });
}

export function useMovie(id: string) {
  return useQuery({
    queryKey: ['movie', id],
    queryFn: () => movieService.getById(id),
    enabled: !!id,
  });
}

export function useMoviesByGenre(genre: string) {
  return useQuery({
    queryKey: ['movies', 'genre', genre],
    queryFn: () => movieService.getByGenre(genre),
    enabled: !!genre,
  });
}

export function useMovieSearch(query: string, params?: Omit<SearchMoviesParams, 'query'>) {
  return useQuery({
    queryKey: ['movies', 'search', query, params],
    queryFn: () => movieService.search(query, params),
    enabled: !!query,
  });
}

export function useGenres() {
  return useQuery({
    queryKey: ['genres'],
    queryFn: () => movieService.getGenres(),
  });
}
