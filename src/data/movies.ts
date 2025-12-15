export interface Movie {
  id: string;
  title: string;
  synopsis: string;
  year: number;
  duration: string;
  genre: string;
  rating: number;
  imageUrl: string;
  backdropUrl: string;
  trailerUrl?: string;
}

export const movies: Movie[] = [
  {
    id: "1",
    title: "Interestelar",
    synopsis: "Uma equipe de exploradores viaja através de um buraco de minhoca no espaço na tentativa de garantir a sobrevivência da humanidade.",
    year: 2014,
    duration: "2h 49min",
    genre: "Ficção Científica",
    rating: 8.7,
    imageUrl: "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/xJHokMbljvjADYdit5fK5VQsXEG.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/zSWdZVtXT7E?rel=0"
  },
  {
    id: "2",
    title: "O Poderoso Chefão",
    synopsis: "O patriarca envelhecido de uma dinastia do crime organizado transfere o controle de seu império clandestino para seu filho relutante.",
    year: 1972,
    duration: "2h 55min",
    genre: "Drama",
    rating: 9.2,
    imageUrl: "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/tmU7GeKVybMWFButWEGl2M4GeiP.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/sY1S34973zA?rel=0"
  },
  {
    id: "3",
    title: "Clube da Luta",
    synopsis: "Um trabalhador de escritório insone e um fabricante de sabão formam um clube de luta clandestino que evolui para muito mais.",
    year: 1999,
    duration: "2h 19min",
    genre: "Drama",
    rating: 8.8,
    imageUrl: "https://image.tmdb.org/t/p/w500/pB8BM7pdSp6B6Ih7QZ4DrQ3PmJK.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/hZkgoQYus5vegHoetLkCJzb17zJ.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/SUXWAEX2jlg?rel=0"
  },
  {
    id: "4",
    title: "Matrix",
    synopsis: "Um hacker descobre a verdadeira natureza de sua realidade e seu papel na guerra contra seus controladores.",
    year: 1999,
    duration: "2h 16min",
    genre: "Ação",
    rating: 8.7,
    imageUrl: "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/fNG7i7RqMErkcqhohV2a6cV1Ehy.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/vKQi3bBA1y8?rel=0"
  },
  {
    id: "5",
    title: "Pulp Fiction",
    synopsis: "As vidas de dois assassinos da máfia, um boxeador, um gângster e sua esposa se entrelaçam em quatro histórias de violência e redenção.",
    year: 1994,
    duration: "2h 34min",
    genre: "Crime",
    rating: 8.9,
    imageUrl: "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/suaEOtk1N1sgg2MTM7oZd2cfVp3.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/s7EdQ4FqbhY?rel=0"
  },
  {
    id: "6",
    title: "O Senhor dos Anéis: O Retorno do Rei",
    synopsis: "Gandalf e Aragorn lideram o Mundo dos Homens contra o exército de Sauron para desviar seu olhar de Frodo e Sam.",
    year: 2003,
    duration: "3h 21min",
    genre: "Fantasia",
    rating: 9.0,
    imageUrl: "https://image.tmdb.org/t/p/w500/rCzpDGLbOoPwLjy3OAm5NUPOTrC.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/pm0RiwNpSja8gR0BTWpxo5a9Bbl.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/r5X-hFf6Bwo?rel=0"
  },
  {
    id: "7",
    title: "Forrest Gump",
    synopsis: "As presidências de Kennedy e Johnson, os eventos do Vietnã, Watergate e outros acontecimentos se desenrolam pela perspectiva de um homem do Alabama.",
    year: 1994,
    duration: "2h 22min",
    genre: "Drama",
    rating: 8.8,
    imageUrl: "https://image.tmdb.org/t/p/w500/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/3h1JZGDhZ8nzxdgvkxha0qBqi05.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/bLvqoHBptjg?rel=0"
  },
  {
    id: "8",
    title: "Inception",
    synopsis: "Um ladrão que rouba segredos corporativos através da tecnologia de compartilhamento de sonhos recebe a tarefa inversa de plantar uma ideia.",
    year: 2010,
    duration: "2h 28min",
    genre: "Ficção Científica",
    rating: 8.8,
    imageUrl: "https://image.tmdb.org/t/p/w500/edv5CZvWj09upOsy2Y6IwDhK8bt.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/8ZTVqvKDQ8emSGUEMjsS4yHAwrp.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/YoHD9XEInc0?rel=0"
  },
  {
    id: "9",
    title: "O Cavaleiro das Trevas",
    synopsis: "Batman aceita um dos maiores testes psicológicos e físicos de sua capacidade de combater a injustiça quando enfrenta o Coringa.",
    year: 2008,
    duration: "2h 32min",
    genre: "Ação",
    rating: 9.0,
    imageUrl: "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/nMKdUUepR0i5zn0y1T4CsSB5chy.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/EXeTwQWrcwY?rel=0"
  },
  {
    id: "10",
    title: "Parasita",
    synopsis: "Uma família pobre e astuta consegue se infiltrar em uma família rica, levando a consequências inesperadas.",
    year: 2019,
    duration: "2h 12min",
    genre: "Thriller",
    rating: 8.5,
    imageUrl: "https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/TU9NIjwzjoKPwQHoHshkFcQUCG.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/5xH0HfJHsaY?rel=0"
  },
  {
    id: "11",
    title: "Gladiador",
    synopsis: "Um ex-general romano parte para vingar o assassinato de sua família e seu imperador.",
    year: 2000,
    duration: "2h 35min",
    genre: "Ação",
    rating: 8.5,
    imageUrl: "https://image.tmdb.org/t/p/w500/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/hND2bsDlNBMLHtVYBMWjGTdPkRF.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/owK1qxDselE?rel=0"
  },
  {
    id: "12",
    title: "De Volta para o Futuro",
    synopsis: "Marty McFly é transportado acidentalmente para 1955 em um DeLorean que viaja no tempo e precisa garantir que seus pais se encontrem.",
    year: 1985,
    duration: "1h 56min",
    genre: "Aventura",
    rating: 8.5,
    imageUrl: "https://image.tmdb.org/t/p/w500/fNOH9f1aA7XRTzl1sAOx9iF553Q.jpg",
    backdropUrl: "https://image.tmdb.org/t/p/original/x4N74cycZvKu5k3KDORhBYPmIAx.jpg",
    trailerUrl: "https://www.youtube-nocookie.com/embed/qvsgGtivCgs?rel=0"
  }
];

export const genres = [...new Set(movies.map(m => m.genre))];

export const getMoviesByGenre = (genre: string) => 
  movies.filter(m => m.genre === genre);

export const getMovieById = (id: string) => 
  movies.find(m => m.id === id);

export const getFeaturedMovie = () => movies[0];

export const searchMovies = (query: string) =>
  movies.filter(m => 
    m.title.toLowerCase().includes(query.toLowerCase()) ||
    m.genre.toLowerCase().includes(query.toLowerCase())
  );
