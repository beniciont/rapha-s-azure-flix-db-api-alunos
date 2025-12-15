import { Link } from 'react-router-dom';
import { Play, Info } from 'lucide-react';
import { Movie } from '@/data/movies';
import { Button } from '@/components/ui/button';

interface HeroSectionProps {
  movie: Movie;
}

export function HeroSection({ movie }: HeroSectionProps) {
  return (
    <section className="relative h-[70vh] md:h-[85vh] overflow-hidden">
      {/* Background Image */}
      <div className="absolute inset-0">
        <img
          src={movie.backdropUrl}
          alt={movie.title}
          className="h-full w-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-r from-background via-background/60 to-transparent" />
        <div className="absolute inset-0 bg-gradient-to-t from-background via-transparent to-transparent" />
      </div>

      {/* Content */}
      <div className="absolute bottom-0 left-0 right-0 pb-24 md:pb-32">
        <div className="container mx-auto px-4">
          <div className="max-w-2xl animate-fade-in">
            <h1 className="text-4xl md:text-6xl font-bold mb-4 drop-shadow-lg">
              {movie.title}
            </h1>
            
            <div className="flex items-center gap-4 mb-4 text-sm text-muted-foreground">
              <span className="text-primary font-semibold">{movie.rating} ★</span>
              <span>{movie.year}</span>
              <span>{movie.duration}</span>
              <span className="px-2 py-0.5 bg-muted rounded text-xs">{movie.genre}</span>
            </div>
            
            <p className="text-foreground/80 text-base md:text-lg mb-6 line-clamp-3">
              {movie.synopsis}
            </p>

            <div className="flex gap-3">
              <Button size="lg" className="gap-2 hover-glow">
                <Play className="h-5 w-5 fill-current" />
                Alugar
              </Button>
              <Button size="lg" variant="secondary" className="gap-2" asChild>
                <Link to={`/movie/${movie.id}`}>
                  <Info className="h-5 w-5" />
                  Mais Informações
                </Link>
              </Button>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
