using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  public static class GraphContextFactoryLocator {
    // PRAGMA MARK - Static
    private static IGraphContextFactory _factory;
    public static void Provide(IGraphContextFactory factory) {
      GraphContextFactoryLocator._factory = factory;
    }

    public static IGraphContext MakeContext() {
      if (GraphContextFactoryLocator._factory == null) {
        GraphContextFactoryLocator._factory = new DefaultGraphContextFactory();
      }
      return GraphContextFactoryLocator._factory.MakeContext();
    }
  }

  public class DefaultGraphContextFactory : IGraphContextFactory {
    // PRAGMA MARK - IGraphContextFactory Implementation
    public IGraphContext MakeContext() {
      return new GraphContext();
    }
  }
}