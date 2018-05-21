﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UIWeb
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TributacaoEntities : DbContext
    {
        public TributacaoEntities()
            : base("name=TributacaoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<escritoriocontabil> escritoriocontabil { get; set; }
        public virtual DbSet<logradouro> logradouro { get; set; }
        public virtual DbSet<bairro> bairro { get; set; }
        public virtual DbSet<cidade> cidade { get; set; }
        public virtual DbSet<endentrega> endentrega { get; set; }
        public virtual DbSet<facequadra> facequadra { get; set; }
        public virtual DbSet<cidadao> cidadao { get; set; }
        public virtual DbSet<segunda_via_web> segunda_via_web { get; set; }
        public virtual DbSet<mobiliario> mobiliario { get; set; }
        public virtual DbSet<parametros> parametros { get; set; }
        public virtual DbSet<mobiliarioevento> mobiliarioevento { get; set; }
        public virtual DbSet<proprietario> proprietario { get; set; }
        public virtual DbSet<mei> mei { get; set; }
        public virtual DbSet<mobiliarioproprietario> mobiliarioproprietario { get; set; }
        public virtual DbSet<cnaesubclasse> cnaesubclasse { get; set; }
        public virtual DbSet<mobiliariocnae> mobiliariocnae { get; set; }
        public virtual DbSet<DEmpresa> DEmpresa { get; set; }
        public virtual DbSet<vre_atividade> vre_atividade { get; set; }
        public virtual DbSet<vre_empresa> vre_empresa { get; set; }
        public virtual DbSet<vre_licenciamento> vre_licenciamento { get; set; }
        public virtual DbSet<vre_pergunta> vre_pergunta { get; set; }
        public virtual DbSet<vre_socio> vre_socio { get; set; }
        public virtual DbSet<vre_declaracao> vre_declaracao { get; set; }
        public virtual DbSet<sil> sil { get; set; }
        public virtual DbSet<horariofunc> horariofunc { get; set; }
        public virtual DbSet<comercio_eletronico> comercio_eletronico { get; set; }
        public virtual DbSet<boleto> boleto { get; set; }
        public virtual DbSet<boletoguia> boletoguia { get; set; }
        public virtual DbSet<debitoparcela> debitoparcela { get; set; }
        public virtual DbSet<debitotributo> debitotributo { get; set; }
        public virtual DbSet<numdocumento> numdocumento { get; set; }
        public virtual DbSet<laseriptu> laseriptu { get; set; }
        public virtual DbSet<parceladocumento> parceladocumento { get; set; }
        public virtual DbSet<mobiliarioatividadevs2> mobiliarioatividadevs2 { get; set; }
        public virtual DbSet<mobiliarioatividadeiss> mobiliarioatividadeiss { get; set; }
        public virtual DbSet<cadimob> cadimob { get; set; }
        public virtual DbSet<condominio> condominio { get; set; }
        public virtual DbSet<cep> cep { get; set; }
    
        public virtual int spEXTRATONEW(Nullable<int> codReduz1, Nullable<int> codReduz2, Nullable<short> anoExercicio1, Nullable<short> anoExercicio2, Nullable<short> codLancamento1, Nullable<short> codLancamento2, Nullable<short> seqLancamento1, Nullable<short> seqLancamento2, Nullable<short> numParcela1, Nullable<short> numParcela2, Nullable<short> codComplemento1, Nullable<short> codComplemento2, Nullable<short> statusLanc1, Nullable<short> statusLanc2, Nullable<System.DateTime> dataNow, string usuario, Nullable<byte> ajuizado)
        {
            var codReduz1Parameter = codReduz1.HasValue ?
                new ObjectParameter("CodReduz1", codReduz1) :
                new ObjectParameter("CodReduz1", typeof(int));
    
            var codReduz2Parameter = codReduz2.HasValue ?
                new ObjectParameter("CodReduz2", codReduz2) :
                new ObjectParameter("CodReduz2", typeof(int));
    
            var anoExercicio1Parameter = anoExercicio1.HasValue ?
                new ObjectParameter("AnoExercicio1", anoExercicio1) :
                new ObjectParameter("AnoExercicio1", typeof(short));
    
            var anoExercicio2Parameter = anoExercicio2.HasValue ?
                new ObjectParameter("AnoExercicio2", anoExercicio2) :
                new ObjectParameter("AnoExercicio2", typeof(short));
    
            var codLancamento1Parameter = codLancamento1.HasValue ?
                new ObjectParameter("CodLancamento1", codLancamento1) :
                new ObjectParameter("CodLancamento1", typeof(short));
    
            var codLancamento2Parameter = codLancamento2.HasValue ?
                new ObjectParameter("CodLancamento2", codLancamento2) :
                new ObjectParameter("CodLancamento2", typeof(short));
    
            var seqLancamento1Parameter = seqLancamento1.HasValue ?
                new ObjectParameter("SeqLancamento1", seqLancamento1) :
                new ObjectParameter("SeqLancamento1", typeof(short));
    
            var seqLancamento2Parameter = seqLancamento2.HasValue ?
                new ObjectParameter("SeqLancamento2", seqLancamento2) :
                new ObjectParameter("SeqLancamento2", typeof(short));
    
            var numParcela1Parameter = numParcela1.HasValue ?
                new ObjectParameter("NumParcela1", numParcela1) :
                new ObjectParameter("NumParcela1", typeof(short));
    
            var numParcela2Parameter = numParcela2.HasValue ?
                new ObjectParameter("NumParcela2", numParcela2) :
                new ObjectParameter("NumParcela2", typeof(short));
    
            var codComplemento1Parameter = codComplemento1.HasValue ?
                new ObjectParameter("CodComplemento1", codComplemento1) :
                new ObjectParameter("CodComplemento1", typeof(short));
    
            var codComplemento2Parameter = codComplemento2.HasValue ?
                new ObjectParameter("CodComplemento2", codComplemento2) :
                new ObjectParameter("CodComplemento2", typeof(short));
    
            var statusLanc1Parameter = statusLanc1.HasValue ?
                new ObjectParameter("StatusLanc1", statusLanc1) :
                new ObjectParameter("StatusLanc1", typeof(short));
    
            var statusLanc2Parameter = statusLanc2.HasValue ?
                new ObjectParameter("StatusLanc2", statusLanc2) :
                new ObjectParameter("StatusLanc2", typeof(short));
    
            var dataNowParameter = dataNow.HasValue ?
                new ObjectParameter("DataNow", dataNow) :
                new ObjectParameter("DataNow", typeof(System.DateTime));
    
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));
    
            var ajuizadoParameter = ajuizado.HasValue ?
                new ObjectParameter("Ajuizado", ajuizado) :
                new ObjectParameter("Ajuizado", typeof(byte));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spEXTRATONEW", codReduz1Parameter, codReduz2Parameter, anoExercicio1Parameter, anoExercicio2Parameter, codLancamento1Parameter, codLancamento2Parameter, seqLancamento1Parameter, seqLancamento2Parameter, numParcela1Parameter, numParcela2Parameter, codComplemento1Parameter, codComplemento2Parameter, statusLanc1Parameter, statusLanc2Parameter, dataNowParameter, usuarioParameter, ajuizadoParameter);
        }
    }
}
