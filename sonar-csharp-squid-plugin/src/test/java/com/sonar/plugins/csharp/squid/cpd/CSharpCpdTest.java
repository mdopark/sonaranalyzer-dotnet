/*
 * Copyright (C) 2009-2012 SonarSource SA
 * All rights reserved
 * mailto:contact AT sonarsource DOT com
 */
package com.sonar.plugins.csharp.squid.cpd;

import static org.hamcrest.Matchers.is;
import static org.junit.Assert.assertThat;

import java.io.File;
import java.io.IOException;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import net.sourceforge.pmd.cpd.AbstractLanguage;
import net.sourceforge.pmd.cpd.TokenEntry;

import org.apache.commons.io.FileUtils;
import org.junit.Test;
import org.sonar.duplications.cpd.CPD;
import org.sonar.duplications.cpd.Match;

public class CSharpCpdTest {

  @Test
  public void testDuplicationsDetection() throws IOException {
    TokenEntry.clearImages();
    CPD cpd = configureCPD(15, true);
    cpd.add(readFile("/cpd/NUnitFramework.cs"));
    cpd.go();
    List<Match> matches = getMatches(cpd);
    assertThat(matches.size(), is(3));
    // 3 lines, 19 tokens
    assertThat(matches.get(0).getFirstMark().getBeginLine(), is(216));
    assertThat(matches.get(0).getSecondMark().getBeginLine(), is(219));
    // 4 lines, 17 tokens
    assertThat(matches.get(1).getFirstMark().getBeginLine(), is(193));
    assertThat(matches.get(1).getSecondMark().getBeginLine(), is(203));
    // 3 lines, 16 tokens
    assertThat(matches.get(2).getFirstMark().getBeginLine(), is(122));
    assertThat(matches.get(2).getSecondMark().getBeginLine(), is(134));
  }

  @Test
  public void testDetectionWithoutIgnoringLiterals() throws IOException {
    // First: we ignore literals, and find a duplication
    TokenEntry.clearImages();
    CPD cpd = configureCPD(70, true);
    cpd.add(readFile("/cpd/duplication-cSharpExample.cs"));
    cpd.go();
    assertThat(getMatches(cpd).size(), is(1));
    // Then, we decide not to ignore literals: no duplication is found anymore
    TokenEntry.clearImages();
    cpd = configureCPD(70, false);
    cpd.add(readFile("/cpd/duplication-cSharpExample.cs"));
    cpd.go();
    assertThat(getMatches(cpd).size(), is(0));
  }

  private File readFile(String path) {
    return FileUtils.toFile(getClass().getResource(path));
  }

  private List<Match> getMatches(CPD cpd) {
    List<Match> matchers = new ArrayList<Match>();

    Iterator<Match> matIterator = cpd.getMatches();
    while (matIterator.hasNext()) {
      matchers.add(matIterator.next());
    }
    return matchers;
  }

  private CPD configureCPD(int minimumTileSize, boolean ignoreLiterals) {
    AbstractLanguage cSharpLanguage = new AbstractLanguage(new CSharpCPDTokenizer(ignoreLiterals, Charset.forName("UTF-8")), "csharp") {

    };

    CPD cpd = new CPD(minimumTileSize, cSharpLanguage);
    cpd.setEncoding("UTF-8");
    cpd.setLoadSourceCodeSlices(false);
    return cpd;
  }
}