using AutoMapper;
using Dikatita.App.Models;
using Dikatita.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Controllers
{
    [Route("catalogo/[action]")]
    public class CatalogoController(IProdutoRepository produtoRepository, IMapper mapper) : BaseController
    {
        [HttpGet("catalogo-produtos")]
        public async Task<IActionResult> Index()
        {
            var produtos = mapper.Map<IEnumerable<ProdutoViewModel>>(await produtoRepository.ObterTodos());
            return View(produtos);
        }

        [HttpGet("detalhes-produto/{id:guid}")]
        public async Task<IActionResult> Detalhes(Guid id)
        {
            var produtoViewModel = mapper.Map<ProdutoViewModel>(await produtoRepository.ObterPorId(id));
            if (produtoViewModel == null) return NotFound();
            return View(produtoViewModel);
        }
    }
}
