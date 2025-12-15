import { useSearchParams } from 'react-router-dom';
import { Header } from '@/components/Header';
import { MovieCard } from '@/components/MovieCard';
import { searchMovies } from '@/data/movies';

export default function Search() {
  const [searchParams] = useSearchParams();
  const query = searchParams.get('q') || '';
  const results = searchMovies(query);

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main className="container mx-auto px-4 pt-24 pb-12">
        <h1 className="text-3xl font-bold mb-2">Resultados da busca</h1>
        <p className="text-muted-foreground mb-8">
          {results.length} resultado(s) para "{query}"
        </p>

        {results.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground text-lg">Nenhum filme encontrado.</p>
          </div>
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
            {results.map((movie) => (
              <MovieCard key={movie.id} movie={movie} />
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
