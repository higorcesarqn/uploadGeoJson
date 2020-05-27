using Core.Models;
using System;
using Xunit;

namespace Core.Tests.ModelsTests
{
    public class EnumerationTests
    {
        [Fact]
        [Trait("Core", "Models.Enumeration")]
        public void criar_enumeration_a_partir_do_id()
        {
            var all = Enumeration.GetAll<State>();

            foreach (var item in all)
            {
                var state = Enumeration.FromValue<State>(item.Id);
                Assert.Equal(state.Name, item.Name);
            }
        }

        [Fact]
        [Trait("Core", "Models.Enumeration")]
        public void criar_enumeration_a_partir_do_nome()
        {
            var all = Enumeration.GetAll<State>();

            foreach (var item in all)
            {
                var state = Enumeration.FromDisplayName<State>(item.Name);
                Assert.Equal(state.Id, item.Id);
            }
        }

        [Fact]
        [Trait("Core", "Models.Enumeration")]
        public void throwns_InvalidOperationException_quando_busca_por_valor_invalido()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Enumeration.FromValue<State>(10));
        }

        [Fact]
        [Trait("Core", "Models.Enumeration")]
        public void throwns_InvalidOperationException_quando_busca_por_nome_invalido()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => Enumeration.FromDisplayName<State>("New York"));
        }
    }

    public class State : Enumeration
    {
        protected State(int id, string name) : base(id, name) { }

        public static State Alabama = new AlabamaState();
        public static State Alaska = new AlaskaState();
        public static State Arizona = new ArizonaState();
        public static State California = new CaliforniaState();

        private class AlabamaState : State
        {
            public AlabamaState() : base(7, "Alabama") { }
        }

        private class AlaskaState : State
        {
            public AlaskaState() : base(1, "Alaska") { }
        }

        private class ArizonaState : State
        {
            public ArizonaState() : base(9, "Arizona") { }
        }

        private class CaliforniaState : State
        {
            public CaliforniaState() : base(53, "California") { }
        }

    }
}
