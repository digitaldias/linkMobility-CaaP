using Moq;
using StructureMap.AutoMocking.Moq;

namespace Link.CrossCutting
{
    public class TestsFor<TEntity> where TEntity : class
    {
        protected TEntity Instance { get; set; }

        protected MoqAutoMocker<TEntity> AutoMock { get; set; }


        public TestsFor()
        {
            AutoMock = new MoqAutoMocker<TEntity>();

            OverrideMocks();

            Instance = AutoMock.ClassUnderTest;
        }


        public virtual void OverrideMocks() {  
        }


        public void Inject<TContract>(TContract with) where TContract : class
        {
            AutoMock.Container.Release(AutoMock.Get<TContract>());
            AutoMock.Inject<TContract>(with);            
        }


        public Mock<TContract> GetMockFor<TContract>() where TContract : class
        {
            return Mock.Get(AutoMock.Get<TContract>());
        }
    }
}
