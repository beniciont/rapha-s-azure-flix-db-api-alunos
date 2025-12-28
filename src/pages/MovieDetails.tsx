import { useParams, useNavigate } from 'react-router-dom';
import { ArrowLeft, Play, Star, Clock, Calendar, Check, Loader2 } from 'lucide-react';
import { Header } from '@/components/Header';
import { Button } from '@/components/ui/button';
import { useMovie, useMoviesByGenre } from '@/hooks/useMovies';
import { useRentals, useCreateRental } from '@/hooks/useRentals';
import { useAuth } from '@/contexts/AuthContext';
import { MovieCarousel } from '@/components/MovieCarousel';
import { toast } from 'sonner';

export default function MovieDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { isAuthenticated, isLoading: authLoading } = useAuth();
  
  const { data: movie, isLoading } = useMovie(id || '');
  // Only fetch rentals when user is authenticated and auth state is loaded
  const { data: rentals } = useRentals(isAuthenticated && !authLoading);
  const createRental = useCreateRental();

  const alreadyRented = rentals?.some(
    r => r.movieId === id && r.status === 'active'
  ) ?? false;

  const { data: relatedMovies } = useMoviesByGenre(movie?.genre || '');
  const filteredRelated = relatedMovies?.filter(m => m.id !== id)?.slice(0, 6) ?? [];

  if (isLoading) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <div className="container mx-auto px-4 pt-24 text-center">
          <Loader2 className="h-8 w-8 animate-spin mx-auto" />
        </div>
      </div>
    );
  }

  if (!movie) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <div className="container mx-auto px-4 pt-24 text-center">
          <h1 className="text-2xl font-bold mb-4">Filme não encontrado</h1>
          <Button onClick={() => navigate('/')}>Voltar ao Início</Button>
        </div>
      </div>
    );
  }

  const handleRent = () => {
    if (!isAuthenticated) {
      toast.error('Faça login para alugar filmes');
      navigate('/auth');
      return;
    }
    
    if (alreadyRented) return;
    createRental.mutate(movie.id);
  };

  const trailerWatchUrl = movie.trailerUrl
    ? movie.trailerUrl
        .replace("www.youtube-nocookie.com", "www.youtube.com")
        .replace("/embed/", "/watch?v=")
    : undefined;

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      {/* Hero Backdrop */}
      <div className="relative h-[50vh] md:h-[60vh]">
        <img
          src={movie.backdropUrl}
          alt={movie.title}
          className="h-full w-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-t from-background via-background/50 to-transparent" />
        <div className="absolute inset-0 bg-gradient-to-r from-background via-transparent to-transparent" />
        
        <Button
          variant="ghost"
          size="icon"
          className="absolute top-20 left-4 z-10"
          onClick={() => navigate(-1)}
        >
          <ArrowLeft className="h-6 w-6" />
        </Button>
      </div>

      {/* Content */}
      <div className="container mx-auto px-4 -mt-48 relative z-10">
        <div className="flex flex-col md:flex-row gap-8">
          {/* Poster */}
          <div className="flex-shrink-0 mx-auto md:mx-0">
            <img
              src={movie.imageUrl}
              alt={movie.title}
              className="w-48 md:w-64 rounded-xl shadow-2xl hover-glow"
            />
          </div>

          {/* Info */}
          <div className="flex-1 text-center md:text-left">
            <h1 className="text-3xl md:text-5xl font-bold mb-4">{movie.title}</h1>
            
            <div className="flex flex-wrap items-center justify-center md:justify-start gap-4 mb-6 text-muted-foreground">
              <div className="flex items-center gap-1">
                <Star className="h-5 w-5 fill-primary text-primary" />
                <span className="font-semibold text-foreground">{movie.rating}</span>
              </div>
              <div className="flex items-center gap-1">
                <Calendar className="h-4 w-4" />
                <span>{movie.year}</span>
              </div>
              <div className="flex items-center gap-1">
                <Clock className="h-4 w-4" />
                <span>{movie.duration}</span>
              </div>
              <span className="px-3 py-1 bg-secondary/20 text-secondary rounded-full text-sm">
                {movie.genre}
              </span>
            </div>

            <p className="text-foreground/80 text-lg mb-8 max-w-2xl">
              {movie.synopsis}
            </p>

            <div className="flex flex-wrap gap-4 justify-center md:justify-start">
              <Button 
                size="lg" 
                className="gap-2 hover-glow" 
                onClick={handleRent}
                disabled={alreadyRented || createRental.isPending}
                variant={alreadyRented ? "secondary" : "default"}
              >
                {createRental.isPending ? (
                  <>
                    <Loader2 className="h-5 w-5 animate-spin" />
                    Processando...
                  </>
                ) : alreadyRented ? (
                  <>
                    <Check className="h-5 w-5" />
                    Já Alugado
                  </>
                ) : (
                  <>
                    <Play className="h-5 w-5 fill-current" />
                    Alugar por R$ {movie.rentalPrice?.toFixed(2) || '9,90'}
                  </>
                )}
              </Button>
            </div>

            {/* Trailer */}
            {trailerWatchUrl && (
              <div className="mt-10 max-w-3xl space-y-4">
                <h2 className="text-xl font-semibold mb-4">Trailer</h2>
                <p className="text-sm text-muted-foreground">
                  Assista ao trailer diretamente no YouTube pelo botão abaixo.
                </p>
                <Button asChild variant="default" size="sm" className="bg-red-600 hover:bg-red-700 text-white hover-glow">
                  <a
                    href={trailerWatchUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    aria-label={`Assistir trailer de ${movie.title} no YouTube`}
                  >
                    Assista ao trailer no YouTube
                  </a>
                </Button>
              </div>
            )}
          </div>
        </div>

        {/* Related Movies */}
        {filteredRelated.length > 0 && (
          <div className="mt-16 pb-12">
            <MovieCarousel title="Filmes Relacionados" movies={filteredRelated} />
          </div>
        )}
      </div>
    </div>
  );
}
