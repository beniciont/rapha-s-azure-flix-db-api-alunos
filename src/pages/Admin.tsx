import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Plus, Edit, Trash2, ArrowLeft, Save } from 'lucide-react';
import { Header } from '@/components/Header';
import { useAuth } from '@/contexts/AuthContext';
import { movies as initialMovies, Movie, genres } from '@/data/movies';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { toast } from 'sonner';

const emptyMovie: Omit<Movie, 'id'> = {
  title: '',
  synopsis: '',
  year: new Date().getFullYear(),
  duration: '',
  genre: '',
  rating: 0,
  imageUrl: '',
  backdropUrl: ''
};

export default function Admin() {
  const { user, isAdmin } = useAuth();
  const navigate = useNavigate();
  const [movies, setMovies] = useState<Movie[]>(initialMovies);
  const [editingMovie, setEditingMovie] = useState<Movie | null>(null);
  const [newMovie, setNewMovie] = useState<Omit<Movie, 'id'>>(emptyMovie);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [isEditing, setIsEditing] = useState(false);

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

  const handleSave = () => {
    if (isEditing && editingMovie) {
      setMovies(movies.map(m => m.id === editingMovie.id ? editingMovie : m));
      toast.success('Filme atualizado com sucesso!');
    } else {
      const movie: Movie = {
        ...newMovie,
        id: String(Date.now())
      };
      setMovies([...movies, movie]);
      toast.success('Filme adicionado com sucesso!');
    }
    setDialogOpen(false);
    setEditingMovie(null);
    setNewMovie(emptyMovie);
    setIsEditing(false);
  };

  const handleEdit = (movie: Movie) => {
    setEditingMovie(movie);
    setIsEditing(true);
    setDialogOpen(true);
  };

  const handleDelete = (id: string) => {
    setMovies(movies.filter(m => m.id !== id));
    toast.success('Filme removido com sucesso!');
  };

  const handleAdd = () => {
    setEditingMovie(null);
    setNewMovie(emptyMovie);
    setIsEditing(false);
    setDialogOpen(true);
  };

  const currentMovie = isEditing ? editingMovie : newMovie;
  const setCurrentMovie = isEditing 
    ? (movie: Movie | Omit<Movie, 'id'>) => setEditingMovie(movie as Movie)
    : setNewMovie;

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
                      value={currentMovie?.title || ''}
                      onChange={(e) => setCurrentMovie({ ...currentMovie!, title: e.target.value })}
                      placeholder="Nome do filme"
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Gênero</Label>
                    <Select
                      value={currentMovie?.genre || ''}
                      onValueChange={(value) => setCurrentMovie({ ...currentMovie!, genre: value })}
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
                    value={currentMovie?.synopsis || ''}
                    onChange={(e) => setCurrentMovie({ ...currentMovie!, synopsis: e.target.value })}
                    placeholder="Descrição do filme"
                    rows={3}
                  />
                </div>

                <div className="grid grid-cols-3 gap-4">
                  <div className="space-y-2">
                    <Label>Ano</Label>
                    <Input
                      type="number"
                      value={currentMovie?.year || ''}
                      onChange={(e) => setCurrentMovie({ ...currentMovie!, year: Number(e.target.value) })}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Duração</Label>
                    <Input
                      value={currentMovie?.duration || ''}
                      onChange={(e) => setCurrentMovie({ ...currentMovie!, duration: e.target.value })}
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
                      value={currentMovie?.rating || ''}
                      onChange={(e) => setCurrentMovie({ ...currentMovie!, rating: Number(e.target.value) })}
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label>URL da Imagem (Poster)</Label>
                  <Input
                    value={currentMovie?.imageUrl || ''}
                    onChange={(e) => setCurrentMovie({ ...currentMovie!, imageUrl: e.target.value })}
                    placeholder="https://..."
                  />
                </div>

                <div className="space-y-2">
                  <Label>URL do Backdrop</Label>
                  <Input
                    value={currentMovie?.backdropUrl || ''}
                    onChange={(e) => setCurrentMovie({ ...currentMovie!, backdropUrl: e.target.value })}
                    placeholder="https://..."
                  />
                </div>

                <Button onClick={handleSave} className="w-full gap-2">
                  <Save className="h-4 w-4" />
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
                <TableHead className="text-right">Ações</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {movies.map((movie) => (
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
              ))}
            </TableBody>
          </Table>
        </div>

        <p className="text-center text-sm text-muted-foreground mt-8">
          ⚠️ Modo demonstração: as alterações são apenas visuais e não persistem após recarregar a página.
        </p>
      </main>
    </div>
  );
}
