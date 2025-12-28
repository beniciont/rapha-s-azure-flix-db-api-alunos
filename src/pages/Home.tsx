import { Header } from '@/components/Header';
import { HeroSection } from '@/components/HeroSection';
import { MovieCarousel } from '@/components/MovieCarousel';
import { Button } from '@/components/ui/button';
import { API_CONFIG } from '@/config/api';
import { useMovies, useGenres } from '@/hooks/useMovies';
import { AlertTriangle, Loader2, RefreshCcw } from 'lucide-react';

export default function Home() {
  const moviesQuery = useMovies({ pageSize: 20 });
  const { data: genres = [] } = useGenres();

  const movies = moviesQuery.data?.data ?? [];
  const featuredMovie = movies[0];

  if (moviesQuery.isLoading) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <main className="flex items-center justify-center h-screen" aria-busy="true">
          <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
        </main>
      </div>
    );
  }

  if (moviesQuery.isError) {
    const message =
      (moviesQuery.error as { message?: string } | null)?.message ||
      'Não foi possível carregar os filmes.';

    return (
      <div className="min-h-screen bg-background">
        <Header />
        <main className="container mx-auto px-4 pt-24 pb-12">
          <section className="max-w-xl mx-auto text-center">
            <div className="inline-flex h-12 w-12 items-center justify-center rounded-full bg-muted mb-4">
              <AlertTriangle className="h-6 w-6 text-muted-foreground" />
            </div>
            <h1 className="text-2xl md:text-3xl font-bold">Não foi possível carregar os filmes</h1>
            <p className="text-muted-foreground mt-2">{message}</p>
            <p className="text-xs text-muted-foreground mt-4 break-all">
              API configurada: {API_CONFIG.BASE_URL}
            </p>
            <Button className="mt-6 gap-2" onClick={() => moviesQuery.refetch()}>
              <RefreshCcw className="h-4 w-4" />
              Tentar novamente
            </Button>
          </section>
        </main>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-background">
      <Header />

      <main>
        {featuredMovie && <HeroSection movie={featuredMovie} />}

        <div className="container mx-auto py-8 space-y-10 -mt-16 relative z-10">
          {movies.length === 0 ? (
            <section className="px-4 md:px-0">
              <h1 className="text-2xl md:text-3xl font-bold">Catálogo de filmes</h1>
              <p className="text-muted-foreground mt-2">Nenhum filme encontrado.</p>
            </section>
          ) : (
            <>
              <MovieCarousel title="Populares" movies={movies} />

              {genres.slice(0, 5).map((genre) => {
                const genreMovies = movies.filter((m) => m.genre === genre);
                if (genreMovies.length === 0) return null;
                return <MovieCarousel key={genre} title={genre} movies={genreMovies} />;
              })}
            </>
          )}
        </div>
      </main>
    </div>
  );
}

