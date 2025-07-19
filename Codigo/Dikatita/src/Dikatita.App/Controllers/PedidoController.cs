using System.Text;
using AutoMapper;
using Dikatita.App.Extensions;
using Dikatita.App.Models;
using Dikatita.App.ViewModels;
using Dikatita.Business.Interfaces;
using Dikatita.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dikatita.App.Controllers;

public class PedidoController(IPedidoRepository _pedidoRepository, IMapper _mapper) : BaseController
{
    private readonly string CarrinhoSessionKey = "CarrinhoSession";

    [HttpPost]
    public async Task<IActionResult> FinalizarPedido(PedidoViewModel viewModel)
    {
        var carrinho = HttpContext.Session.GetObject<CarrinhoProdutoViewModel>(CarrinhoSessionKey);
        if (carrinho == null || carrinho.Produtos.Count == 0) return RedirectToAction("Index", "Carrinho");
        if (!ModelState.IsValid) return PartialView("_FormularioCliente", viewModel);

        var pedido = new Pedido
        {
            DataCadastro = DateTime.Now,
            NomeCliente = viewModel.NomeCliente,
            TelefoneCliente = viewModel.TelefoneCliente,
            CpfCliente = viewModel.CpfCliente,
            Itens = carrinho.Produtos.Select(p => new ItemPedido
            {
                ProdutoId = p.Id,
                NomeProduto = p.Nome,
                ValorUnitario = p.Valor,
                Quantidade = p.QuantidadeCarrinho
            }).ToList()
        };

        await _pedidoRepository.Adicionar(pedido);
        HttpContext.Session.Remove(CarrinhoSessionKey);

        var mensagem = MontarMensagem(pedido);

        string numeroWhatsApp = "553131667070";
        string urlEncodedMessage = Uri.EscapeDataString(mensagem);
        string linkWhatsApp = $"https://wa.me/{numeroWhatsApp}?text={urlEncodedMessage}";

        return Redirect(linkWhatsApp);
    }

    private static string MontarMensagem(Pedido pedido)
    {
        var mensagem = new StringBuilder();
        mensagem.AppendLine("🛍️ *NOVO PEDIDO RECEBIDO*");
        mensagem.AppendLine($"\n👤 *Nome:* {pedido.NomeCliente}");
        mensagem.AppendLine($"📞 *Telefone:* {pedido.TelefoneCliente}");
        mensagem.AppendLine($"🆔 *CPF:* {pedido.CpfCliente}");
        mensagem.AppendLine("\n📦 *Itens do Carrinho:*");

        foreach (var item in pedido.Itens)
        {
            mensagem.AppendLine($"• {item.NomeProduto} — Qtd: {item.Quantidade} x R$ {item.ValorUnitario:F2}");
        }

        decimal total = pedido.Itens.Sum(i => i.ValorUnitario * i.Quantidade);
        mensagem.AppendLine($"\n💰 *Valor Total:* R$ {total:F2}");

        return mensagem.ToString();
    }
}