import { useState, useEffect } from 'react';
import { Movie } from '@/data/movies';

const RENTALS_KEY = 'rentedMovies';

export function useRentals() {
  const [rentedMovies, setRentedMovies] = useState<Movie[]>([]);

  useEffect(() => {
    const stored = localStorage.getItem(RENTALS_KEY);
    if (stored) {
      try {
        setRentedMovies(JSON.parse(stored));
      } catch {
        setRentedMovies([]);
      }
    }
  }, []);

  const addRental = (movie: Movie) => {
    setRentedMovies((prev) => {
      const exists = prev.some((m) => m.id === movie.id);
      if (exists) return prev;
      const updated = [...prev, movie];
      localStorage.setItem(RENTALS_KEY, JSON.stringify(updated));
      return updated;
    });
  };

  const removeRental = (movieId: string) => {
    setRentedMovies((prev) => {
      const updated = prev.filter((m) => m.id !== movieId);
      localStorage.setItem(RENTALS_KEY, JSON.stringify(updated));
      return updated;
    });
  };

  const isRented = (movieId: string) => {
    return rentedMovies.some((m) => m.id === movieId);
  };

  return { rentedMovies, addRental, removeRental, isRented };
}
