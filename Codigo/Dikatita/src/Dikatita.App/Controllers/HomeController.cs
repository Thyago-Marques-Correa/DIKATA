using Microsoft.AspNetCore.Mvc;
using Dikatita.App.Models;
using Dikatita.Business.Interfaces;
using AutoMapper;
using System.Diagnostics;
using Dikatita.App.ViewModels;
using Dikatita.Business.Models;

namespace Dikatita.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;
        private readonly IPedidoRepository _pedidoRepository;
        
        public HomeController(
            ILogger<HomeController> logger,
            IProdutoRepository produtoRepository,
            IMapper mapper,
            IPedidoRepository pedidoRepository)
        {
            _logger = logger;
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _pedidoRepository = pedidoRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
        
                var produtos = await _produtoRepository.ObterTodos();
                var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);

         
                return View(produtosViewModel);
            }
            catch (Exception ex)
            {
        
                _logger.LogError(ex, "Erro ao carregar produtos na p�gina inicial.");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string termo)
        {
            if (string.IsNullOrEmpty(termo))
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var produtos = await _produtoRepository.BuscarPorNome(termo);
                var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(produtos);
                
                ViewBag.TermoBusca = termo;
                return View("Index", produtosViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produtos.");
                return View("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contatos()
        {
            return View();
        }

        public IActionResult Carrinho()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        public IActionResult Register()
        {
            return RedirectToAction("Register", "Account", new { area = "Identity" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        [HttpPost]
        public async Task<IActionResult> SalvarPedido(PedidoViewModel pedidoViewModel)
        {
            if(!ModelState.IsValid) return View(nameof(Index));
        
            await _pedidoRepository.Adicionar(_mapper.Map<Pedido>(pedidoViewModel));
            return RedirectToAction(nameof(Index));
        }
    }
}
