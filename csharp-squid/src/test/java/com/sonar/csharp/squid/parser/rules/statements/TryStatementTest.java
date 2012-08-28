/*
 * Copyright (C) 2009-2012 SonarSource SA
 * All rights reserved
 * mailto:contact AT sonarsource DOT com
 */
package com.sonar.csharp.squid.parser.rules.statements;

import static com.sonar.sslr.test.parser.ParserMatchers.*;
import static org.junit.Assert.*;

import java.nio.charset.Charset;

import org.junit.Before;
import org.junit.Test;

import com.sonar.csharp.squid.CSharpConfiguration;
import com.sonar.csharp.squid.api.CSharpGrammar;
import com.sonar.csharp.squid.parser.CSharpParser;
import com.sonar.sslr.impl.Parser;

public class TryStatementTest {

  private final Parser<CSharpGrammar> p = CSharpParser.create(new CSharpConfiguration(Charset.forName("UTF-8")));
  private final CSharpGrammar g = p.getGrammar();

  @Before
  public void init() {
    p.setRootRule(g.tryStatement);
  }

  @Test
  public void testOk() {
    g.block.mock();
    g.catchClauses.mock();
    g.finallyClause.mock();
    assertThat(p, parse("try block catchClauses"));
    assertThat(p, parse("try block finallyClause"));
    assertThat(p, parse("try block catchClauses finallyClause"));
  }

  @Test
  public void testRealLife() throws Exception {
    assertThat(p,
        parse("try { RegisterAppDomainEvents(); } catch(System.Security.SecurityException)  { LogLog.Debug(\"LoggerManager\"); }"));
  }

}