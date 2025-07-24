﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaphiraTerror.Models
{
    [Table("Classificacao")]
    public class Classificacao
    {
            [Key]
            public int IdClassificacao { get; set; }
            [Required(ErrorMessage = "Campo obrigatório!")]
            [Display(Name = "Classificação")]
            [StringLength(150)]
            public string DescricaoClassificacao { get; set; }
            public List<Filme> Filmes { get; set; } = new List<Filme>();
    }
}
