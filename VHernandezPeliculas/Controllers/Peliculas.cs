using Microsoft.AspNetCore.Mvc;
using ML;
using System.Security.Principal;

namespace VHernandezPeliculas.Controllers
{
    public class Peliculas : Controller
    {
        [HttpGet]
        public IActionResult GetRatedMovies()
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Pelis = new List<object>();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responsetask = client.GetAsync("discover/movie?api_key=949930115b9a17ca1c8ff4cf61566cf9&include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc"); 
                responsetask.Wait();
                var result = responsetask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Pelicula>();
                    readTask.Wait();
                    foreach (dynamic resultItem in readTask.Result.results)
                    {
                        ML.Pelicula resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Pelicula>(resultItem.ToString());
                        pelicula.Pelis.Add(resultItemList);
                        
                    }
                    
                }
            }
            return View(pelicula);
        }
        [HttpGet]
        public IActionResult GetFavorites()
        {
            ML.Pelicula pelicula = new ML.Pelicula();
            pelicula.Pelis = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responsetask = client.GetAsync("account/19722869/favorite/movies?api_key=949930115b9a17ca1c8ff4cf61566cf9&page=1&session_id=e82458685f17118a16630b70ca88071e7f6e93a6&sort_by=created_at.asc");
                responsetask.Wait();
                var result = responsetask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask=result.Content.ReadAsAsync<ML.Pelicula>();
                    readtask.Wait();
                    foreach (dynamic item in readtask.Result.results)
                    {
                        ML.Pelicula resultitem=Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Pelicula>(item.ToString());
                        pelicula.Pelis.Add(resultitem);
                    }
                }
            }
            return View(pelicula);
        }
        [HttpGet]
        public IActionResult AddMovie(int media_id)
        {
            ML.Favorito pelicula = new ML.Favorito();
            pelicula.media_id = media_id;
            pelicula.media_type = "movie";
            pelicula.favorite = true;
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responsetask = client.PostAsJsonAsync<ML.Favorito>("account/19722869/favorite?session_id=e82458685f17118a16630b70ca88071e7f6e93a6&api_key=949930115b9a17ca1c8ff4cf61566cf9",pelicula);
                responsetask.Wait();
                var resultask = responsetask.Result;
                if (resultask.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se añadio a favoritos con exito";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error inesperado";
                }
            }
            return View("Modal");
        }
        [HttpGet]
        public IActionResult Delete(int media_id)
        {
            ML.Favorito favorito = new ML.Favorito();
            favorito.media_id = media_id;
            favorito.media_type = "movie";
            favorito.favorite = false;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var responsetask = client.PostAsJsonAsync<ML.Favorito>("account/19722869/favorite?session_id=e82458685f17118a16630b70ca88071e7f6e93a6&api_key=949930115b9a17ca1c8ff4cf61566cf9", favorito);
                responsetask.Wait();
                var resultask = responsetask.Result;
                if (resultask.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se Elimino de favoritos con exito";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error inesperado";
                }
            }
            return View("Modal");
        }
    }

}
