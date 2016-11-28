using System;
using System.Windows;
using Autofac;
using AutoMapper;
using Exercise.UI.Model;
using Exercise.UI.Model.Contract;
using Exercise.UI.Repositories;
using Exercise.UI.ViewModel;
using Exercise.UI.ViewModel.Factories;

namespace Exercise.UI.Infrastructure
{
    public class Bootstrapper
    {
        private readonly Lazy<IContainer> _container;
        public IContainer Container => _container.Value;

        public Bootstrapper()
        {
            _container = new Lazy<IContainer>(RegisterContainer);
        }

        private IContainer RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<StockService>().As<IStockService>();
            builder.RegisterType<StockReposiotry>().As<IStockReposiotry>();
            builder.RegisterType<MapperFactory>().As<IFactory<IMapper>>();
            builder.Register(_ => Container.Resolve<IFactory<IMapper>>().Create());
            builder.RegisterType<FundViewModelFactory>().As<IFactory<FundViewModel>>();
            builder.RegisterType<FundWindow>().As<Window>();
            builder.RegisterType<StockConfiguration>().As<IStockConfiguration>();
            builder.RegisterType<StockFactory>().As<IFactory<Stock>>();

            return builder.Build();
        }
    }
}