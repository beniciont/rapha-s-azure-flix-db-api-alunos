import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { rentalService } from '@/services/rentalService';
import { authService } from '@/services/authService';
import { toast } from 'sonner';
import { ApiError } from '@/types/api';

export function useRentals() {
  const isAuthenticated = authService.isAuthenticated();
  
  return useQuery({
    queryKey: ['rentals', 'user'],
    queryFn: () => rentalService.getUserRentals(),
    enabled: isAuthenticated,
  });
}

export function useRental(id: string) {
  return useQuery({
    queryKey: ['rental', id],
    queryFn: () => rentalService.getById(id),
    enabled: !!id,
  });
}

export function useCreateRental() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (movieId: string) => rentalService.create({ movieId }),
    onSuccess: (rental) => {
      queryClient.invalidateQueries({ queryKey: ['rentals'] });
      toast.success(`Filme alugado com sucesso! Devolução até ${new Date(rental.dueDate).toLocaleDateString('pt-BR')}`);
    },
    onError: (error: ApiError) => {
      toast.error(error.message || 'Erro ao alugar filme');
    },
  });
}

export function useReturnRental() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (rentalId: string) => rentalService.returnRental(rentalId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['rentals'] });
      toast.success('Filme devolvido com sucesso!');
    },
    onError: (error: ApiError) => {
      toast.error(error.message || 'Erro ao devolver filme');
    },
  });
}

export function useIsMovieRented(movieId: string) {
  const { data: rentals } = useRentals();
  
  return rentals?.some(
    rental => rental.movieId === movieId && rental.status === 'active'
  ) ?? false;
}
