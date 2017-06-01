using Moq;
using StructureMap.AutoMocking.Moq;

namespace Link.CrossCutting
{
    public class TestsFor<TEntity> where TEntity : class
    {
        public TEntity Instance { get; set; }

        public MoqAutoMocker<TEntity> AutoMock { get; set; }


        public TestsFor()
        {
            AutoMock = new MoqAutoMocker<TEntity>();

            Instance = AutoMock.ClassUnderTest;
        }


        public Mock<TContract> GetMockFor<TContract>() where TContract : class
        {
            return Mock.Get(AutoMock.Get<TContract>());
        }
    }
}
