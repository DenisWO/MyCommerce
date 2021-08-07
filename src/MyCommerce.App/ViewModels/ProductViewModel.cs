using Microsoft.AspNetCore.Http;
using MyCommerce.App.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCommerce.App.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="O campo {0} é obrigatório")]
        [DisplayName("Fornecedor")]
        public Guid ProviderId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Description { get; set; }        
        public string Image { get; set; }

        [NotMappedAttribute]
        [DisplayName("Imagem do Produto")]
        public IFormFile ImageUpload { get; set; }

        [DisplayName("Preço")]
        [Coin]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Price { get; set; }

        [DisplayName("Data de Cadastro")]
        [ScaffoldColumn(false)]        
        public DateTime RegisterDate { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        [DisplayName("Fornecedor")]
        public ProviderViewModel Provider { get; set; }

        public IEnumerable<ProviderViewModel> Providers { get; set; }
    }
}
