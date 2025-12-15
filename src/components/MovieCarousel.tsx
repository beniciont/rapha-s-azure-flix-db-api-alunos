import { useRef } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';
import { Movie } from '@/types/api';
import { MovieCard } from './MovieCard';
import { Button } from '@/components/ui/button';

interface MovieCarouselProps {
  title: string;
  movies: Movie[];
}

export function MovieCarousel({ title, movies }: MovieCarouselProps) {
  const scrollRef = useRef<HTMLDivElement>(null);

  const scroll = (direction: 'left' | 'right') => {
    if (scrollRef.current) {
      const scrollAmount = scrollRef.current.clientWidth * 0.8;
      scrollRef.current.scrollBy({
        left: direction === 'left' ? -scrollAmount : scrollAmount,
        behavior: 'smooth'
      });
    }
  };

  if (movies.length === 0) return null;

  return (
    <section className="relative group/carousel">
      <h2 className="text-xl md:text-2xl font-bold mb-4 px-4 md:px-0">{title}</h2>
      
      <div className="relative">
        {/* Left Arrow */}
        <Button
          variant="ghost"
          size="icon"
          className="absolute left-0 top-1/2 -translate-y-1/2 z-10 h-full rounded-none bg-gradient-to-r from-background to-transparent opacity-0 group-hover/carousel:opacity-100 transition-opacity"
          onClick={() => scroll('left')}
        >
          <ChevronLeft className="h-8 w-8" />
        </Button>

        {/* Movies Grid */}
        <div
          ref={scrollRef}
          className="flex gap-3 overflow-x-auto scrollbar-hide px-4 md:px-0 pb-4"
        >
          {movies.map((movie) => (
            <div key={movie.id} className="flex-shrink-0 w-[140px] md:w-[180px]">
              <MovieCard movie={movie} />
            </div>
          ))}
        </div>

        {/* Right Arrow */}
        <Button
          variant="ghost"
          size="icon"
          className="absolute right-0 top-1/2 -translate-y-1/2 z-10 h-full rounded-none bg-gradient-to-l from-background to-transparent opacity-0 group-hover/carousel:opacity-100 transition-opacity"
          onClick={() => scroll('right')}
        >
          <ChevronRight className="h-8 w-8" />
        </Button>
      </div>
    </section>
  );
}
