import { Header } from '@/components/Header';
import { useRentals } from '@/hooks/useRentals';
import { MovieCard } from '@/components/MovieCard';
import { Button } from '@/components/ui/button';
import { Trash2, Film } from 'lucide-react';
import { toast } from 'sonner';

export default function MyRentals() {
  const { rentedMovies, removeRental } = useRentals();

  const handleRemove = (movieId: string, title: string) => {
    removeRental(movieId);
    toast.success(`"${title}" foi removido da sua lista.`);
  };

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 pt-24 pb-12">
        <h1 className="text-3xl font-bold mb-8">Meus Aluguéis</h1>

        {rentedMovies.length === 0 ? (
          <div className="text-center py-16">
            <Film className="h-16 w-16 mx-auto text-muted-foreground mb-4" />
            <p className="text-muted-foreground text-lg">
              Você ainda não alugou nenhum filme.
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-4">
            {rentedMovies.map((movie) => (
              <div key={movie.id} className="relative group">
                <MovieCard movie={movie} />
                <Button
                  variant="destructive"
                  size="icon"
                  className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity"
                  onClick={(e) => {
                    e.preventDefault();
                    e.stopPropagation();
                    handleRemove(movie.id, movie.title);
                  }}
                >
                  <Trash2 className="h-4 w-4" />
                </Button>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}
