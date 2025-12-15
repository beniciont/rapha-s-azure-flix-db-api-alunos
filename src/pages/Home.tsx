import { Header } from '@/components/Header';
import { HeroSection } from '@/components/HeroSection';
import { MovieCarousel } from '@/components/MovieCarousel';
import { movies, genres, getMoviesByGenre, getFeaturedMovie } from '@/data/movies';

export default function Home() {
  const featuredMovie = getFeaturedMovie();

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main>
        <HeroSection movie={featuredMovie} />
        
        <div className="container mx-auto py-8 space-y-10 -mt-16 relative z-10">
          {/* All Movies */}
          <MovieCarousel title="Populares" movies={movies} />
          
          {/* By Genre */}
          {genres.map((genre) => (
            <MovieCarousel
              key={genre}
              title={genre}
              movies={getMoviesByGenre(genre)}
            />
          ))}
        </div>
      </main>
    </div>
  );
}
