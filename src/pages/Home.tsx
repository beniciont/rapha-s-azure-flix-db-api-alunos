import { Header } from '@/components/Header';
import { HeroSection } from '@/components/HeroSection';
import { MovieCarousel } from '@/components/MovieCarousel';
import { useMovies, useGenres } from '@/hooks/useMovies';
import { Loader2 } from 'lucide-react';

export default function Home() {
  const { data: moviesData, isLoading } = useMovies({ pageSize: 20 });
  const { data: genres = [] } = useGenres();

  const movies = moviesData?.data ?? [];
  const featuredMovie = movies[0];

  if (isLoading) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <div className="flex items-center justify-center h-screen">
          <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main>
        {featuredMovie && <HeroSection movie={featuredMovie} />}
        
        <div className="container mx-auto py-8 space-y-10 -mt-16 relative z-10">
          {/* All Movies */}
          <MovieCarousel title="Populares" movies={movies} />
          
          {/* By Genre - we show all movies filtered by genre */}
          {genres.slice(0, 5).map((genre) => {
            const genreMovies = movies.filter(m => m.genre === genre);
            if (genreMovies.length === 0) return null;
            return (
              <MovieCarousel
                key={genre}
                title={genre}
                movies={genreMovies}
              />
            );
          })}
        </div>
      </main>
    </div>
  );
}
