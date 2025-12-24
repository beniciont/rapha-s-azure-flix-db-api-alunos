import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Edit, Trash2, ArrowLeft, Save, Loader2 } from 'lucide-react';
import { Header } from '@/components/Header';
import { useAuth } from '@/contexts/AuthContext';
import { useMovies, useGenres } from '@/hooks/useMovies';
import { movieService } from '@/services/movieService';
import { Movie } from '@/types/api';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { toast } from 'sonner';
import { useQueryClient } from '@tanstack/react-query';

interface MovieForm {
  title: string;
  synopsis: string;
  year: number;
  duration: string;
  genre: string;
  rating: number;
  imageUrl: string;
  backdropUrl: string;
  trailerUrl?: string;
  rentalPrice?: number;
}

const emptyMovie: MovieForm = {
  title: '',
  synopsis: '',
  year: new Date().getFullYear(),
  duration: '',
  genre: '',
  rating: 0,
  imageUrl: '',
  backdropUrl: '',
  trailerUrl: '',
  rentalPrice: 9.90
};

export default function Admin() {
  const { isAdmin } = useAuth();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  
  const { data: moviesData, isLoading } = useMovies({ pageSize: 100 });
  const { data: genres = [] } = useGenres();
  
  const movies = moviesData?.data ?? [];
  
  const [editingMovie, setEditingMovie] = useState<Movie | null>(null);
  const [formData, setFormData] = useState<MovieForm>(emptyMovie);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  if (!isAdmin) {
    return (
      <div className="min-h-screen bg-background">
        <Header />
        <div className="container mx-auto px-4 pt-24 text-center">
          <h1 className="text-2xl font-bold mb-4">Acesso Negado</h1>
          <p className="text-muted-foreground mb-4">Você não tem permissão para acessar esta página.</p>
          <Button onClick={() => navigate('/')}>Voltar ao Início</Button>
        </div>
      </div>
    );
  }

  const handleSave = async () => {
    if (!formData.title || !formData.genre) {
      toast.error('Preencha título e gênero');
      return;
    }

    setIsSaving(true);
    try {
      if (isEditing && editingMovie) {
        await movieService.update({
          id: editingMovie.id,
          ...formData,
          rentalPrice: formData.rentalPrice || 9.90
        });
        toast.success('Filme atualizado com sucesso!');
      } else {
        await movieService.create({
          ...formData,
          rentalPrice: formData.rentalPrice || 9.90
        });
        toast.success('Filme adicionado com sucesso!');
      }
      
      queryClient.invalidateQueries({ queryKey: ['movies'] });
      setDialogOpen(false);
      setEditingMovie(null);
      setFormData(emptyMovie);
      setIsEditing(false);
    } catch (error: any) {
      toast.error(error.message || 'Erro ao salvar filme');
    } finally {
      setIsSaving(false);
    }
  };

  const handleEdit = (movie: Movie) => {
    setEditingMovie(movie);
    setFormData({
      title: movie.title,
      synopsis: movie.synopsis,
      year: movie.year,
      duration: movie.duration,
      genre: movie.genre,
      rating: movie.rating,
      imageUrl: movie.imageUrl,
      backdropUrl: movie.backdropUrl || '',
      trailerUrl: movie.trailerUrl || '',
      rentalPrice: movie.rentalPrice || 9.90
    });
    setIsEditing(true);
    setDialogOpen(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Tem certeza que deseja excluir este filme?')) return;
    
    try {
      await movieService.delete(id);
      queryClient.invalidateQueries({ queryKey: ['movies'] });
      toast.success('Filme removido com sucesso!');
    } catch (error: any) {
      toast.error(error.message || 'Erro ao remover filme');
    }
  };

  const handleAdd = () => {
    setEditingMovie(null);
    setFormData(emptyMovie);
    setIsEditing(false);
    setDialogOpen(true);
  };

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

  return (
    <div className="min-h-screen bg-background">
      <Header />
      
      <main className="container mx-auto px-4 pt-24 pb-12">
        <div className="flex items-center justify-between mb-8">
          <div className="flex items-center gap-4">
            <Button variant="ghost" size="icon" onClick={() => navigate('/')}>
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <h1 className="text-3xl font-bold">Painel Administrativo</h1>
          </div>

          <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
            <DialogTrigger asChild>
              <Button onClick={handleAdd} className="gap-2">
                <Plus className="h-4 w-4" />
                Adicionar Filme
              </Button>
            </DialogTrigger>
            <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
              <DialogHeader>
                <DialogTitle>{isEditing ? 'Editar Filme' : 'Adicionar Novo Filme'}</DialogTitle>
              </DialogHeader>
              
              <div className="grid gap-4 py-4">
                <div className="grid grid-cols-2 gap-4">
                  <div className="space-y-2">
                    <Label>Título</Label>
                    <Input
                      value={formData.title}
                      onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                      placeholder="Nome do filme"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Gênero</Label>
                    <Select
                      value={formData.genre}
                      onValueChange={(value) => setFormData({ ...formData, genre: value })}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Selecione" />
                      </SelectTrigger>
                      <SelectContent>
                        {genres.map((g) => (
                          <SelectItem key={g} value={g}>{g}</SelectItem>
                        ))}
                        <SelectItem value="Outro">Outro</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>

                <div className="space-y-2">
                  <Label>Sinopse</Label>
                  <Textarea
                    value={formData.synopsis}
                    onChange={(e) => setFormData({ ...formData, synopsis: e.target.value })}
                    placeholder="Descrição do filme"
                    rows={3}
                  />
                </div>

                <div className="grid grid-cols-4 gap-4">
                  <div className="space-y-2">
                    <Label>Ano</Label>
                    <Input
                      type="number"
                      value={formData.year}
                      onChange={(e) => setFormData({ ...formData, year: Number(e.target.value) })}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Duração</Label>
                    <Input
                      value={formData.duration}
                      onChange={(e) => setFormData({ ...formData, duration: e.target.value })}
                      placeholder="2h 30min"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Nota</Label>
                    <Input
                      type="number"
                      step="0.1"
                      min="0"
                      max="10"
                      value={formData.rating}
                      onChange={(e) => setFormData({ ...formData, rating: Number(e.target.value) })}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Preço (R$)</Label>
                    <Input
                      type="number"
                      step="0.01"
                      min="0"
                      value={formData.rentalPrice}
                      onChange={(e) => setFormData({ ...formData, rentalPrice: Number(e.target.value) })}
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label>URL da Imagem (Poster)</Label>
                  <Input
                    value={formData.imageUrl}
                    onChange={(e) => setFormData({ ...formData, imageUrl: e.target.value })}
                    placeholder="https://..."
                  />
                </div>

                <div className="space-y-2">
                  <Label>URL do Backdrop</Label>
                  <Input
                    value={formData.backdropUrl}
                    onChange={(e) => setFormData({ ...formData, backdropUrl: e.target.value })}
                    placeholder="https://..."
                  />
                </div>

                <div className="space-y-2">
                  <Label>URL do Trailer (YouTube)</Label>
                  <Input
                    value={formData.trailerUrl}
                    onChange={(e) => setFormData({ ...formData, trailerUrl: e.target.value })}
                    placeholder="https://www.youtube.com/watch?v=..."
                  />
                </div>

                <Button onClick={handleSave} disabled={isSaving} className="w-full gap-2">
                  {isSaving ? (
                    <Loader2 className="h-4 w-4 animate-spin" />
                  ) : (
                    <Save className="h-4 w-4" />
                  )}
                  {isEditing ? 'Salvar Alterações' : 'Adicionar Filme'}
                </Button>
              </div>
            </DialogContent>
          </Dialog>
        </div>

        {/* Movies Table */}
        <div className="rounded-lg border border-border overflow-hidden">
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead className="w-16">Poster</TableHead>
                <TableHead>Título</TableHead>
                <TableHead>Gênero</TableHead>
                <TableHead>Ano</TableHead>
                <TableHead>Nota</TableHead>
                <TableHead>Preço</TableHead>
                <TableHead className="text-right">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {movies.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                    Nenhum filme cadastrado
                  </TableCell>
                </TableRow>
              ) : (
                movies.map((movie) => (
                  <TableRow key={movie.id}>
                    <TableCell>
                      <img
                        src={movie.imageUrl}
                        alt={movie.title}
                        className="w-12 h-16 object-cover rounded"
                      />
                    </TableCell>
                    <TableCell className="font-medium">{movie.title}</TableCell>
                    <TableCell>{movie.genre}</TableCell>
                    <TableCell>{movie.year}</TableCell>
                    <TableCell>{movie.rating}</TableCell>
                    <TableCell>R$ {movie.rentalPrice?.toFixed(2) || '9.90'}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(movie)}>
                          <Edit className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleDelete(movie.id)}>
                          <Trash2 className="h-4 w-4 text-destructive" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        </div>

        <p className="text-center text-sm text-muted-foreground mt-8">
          Total: {movies.length} filmes cadastrados
        </p>
      </main>
    </div>
  );
}