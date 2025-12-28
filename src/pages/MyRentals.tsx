import { Header } from '@/components/Header';
import { useRentals, useReturnRental } from '@/hooks/useRentals';
import { Button } from '@/components/ui/button';
import { Undo2, Film, Calendar, Clock } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';

export default function MyRentals() {
  const { isAuthenticated, isLoading: authLoading } = useAuth();
  const navigate = useNavigate();
  const { data: rentals, isLoading } = useRentals(isAuthenticated && !authLoading);
  const returnRental = useReturnRental();

  if (!isAuthenticated) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <main className="container mx-auto px-4 pt-24 pb-12 text-center">
          <Film className="h-16 w-16 mx-auto text-muted-foreground mb-4" />
          <h1 className="text-2xl font-bold mb-4">Faça login para ver seus aluguéis</h1>
          <Button onClick={() => navigate('/auth')}>Entrar</Button>
        </main>
      </div>
    );
  }

  const activeRentals = rentals?.filter(r => r.status === 'active') ?? [];
  const pastRentals = rentals?.filter(r => r.status !== 'active') ?? [];

  return (
    <div className="min-h-screen bg-background">
      <Header />
      <main className="container mx-auto px-4 pt-24 pb-12">
        <h1 className="text-3xl font-bold mb-8">Meus Aluguéis</h1>

        {isLoading ? (
          <div className="text-center py-16">
            <p className="text-muted-foreground">Carregando...</p>
          </div>
        ) : activeRentals.length === 0 && pastRentals.length === 0 ? (
          <div className="text-center py-16">
            <Film className="h-16 w-16 mx-auto text-muted-foreground mb-4" />
            <p className="text-muted-foreground text-lg mb-4">
              Você ainda não alugou nenhum filme.
            </p>
            <Button onClick={() => navigate('/catalog')}>Ver Catálogo</Button>
          </div>
        ) : (
          <div className="space-y-8">
            {activeRentals.length > 0 && (
              <div>
                <h2 className="text-xl font-semibold mb-4 flex items-center gap-2">
                  <Clock className="h-5 w-5 text-primary" />
                  Aluguéis Ativos ({activeRentals.length})
                </h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                  {activeRentals.map((rental) => (
                    <div
                      key={rental.id}
                      className="bg-card border border-border rounded-lg p-4 flex gap-4"
                    >
                      {rental.movie && (
                        <Link to={`/movie/${rental.movie.id}`}>
                          <img
                            src={rental.movie.imageUrl}
                            alt={rental.movie.title}
                            className="w-20 h-28 object-cover rounded"
                          />
                        </Link>
                      )}
                      <div className="flex-1 flex flex-col justify-between">
                        <div>
                          <h3 className="font-semibold">{rental.movie?.title}</h3>
                          <p className="text-sm text-muted-foreground flex items-center gap-1 mt-1">
                            <Calendar className="h-3 w-3" />
                            Devolução: {new Date(rental.dueDate).toLocaleDateString('pt-BR')}
                          </p>
                        </div>
                        <Button
                          variant="outline"
                          size="sm"
                          className="mt-2 gap-1"
                          onClick={() => returnRental.mutate(rental.id)}
                          disabled={returnRental.isPending}
                        >
                          <Undo2 className="h-4 w-4" />
                          Devolver
                        </Button>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {pastRentals.length > 0 && (
              <div>
                <h2 className="text-xl font-semibold mb-4 text-muted-foreground">
                  Histórico ({pastRentals.length})
                </h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 opacity-60">
                  {pastRentals.map((rental) => (
                    <div
                      key={rental.id}
                      className="bg-card border border-border rounded-lg p-4 flex gap-4"
                    >
                      {rental.movie && (
                        <img
                          src={rental.movie.imageUrl}
                          alt={rental.movie.title}
                          className="w-16 h-24 object-cover rounded"
                        />
                      )}
                      <div>
                        <h3 className="font-medium">{rental.movie?.title}</h3>
                        <p className="text-sm text-muted-foreground">
                          Devolvido em: {rental.returnedAt ? new Date(rental.returnedAt).toLocaleDateString('pt-BR') : 'N/A'}
                        </p>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        )}
      </main>
    </div>
  );
}
